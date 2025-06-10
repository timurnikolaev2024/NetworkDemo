using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Infrastructure
{
    public sealed class RequestQueue : IDisposable
    {
        interface IQueuedRequest { UniTask ExecuteAsync(); }

        sealed class QueuedRequest<T> : IQueuedRequest
        {
            readonly IRequest<T> _request;
            readonly UniTaskCompletionSource<T> _tcs;
            readonly CancellationToken _token;

            public QueuedRequest(IRequest<T> request, UniTaskCompletionSource<T> tcs, CancellationToken token)
            {
                _request = request;
                _tcs = tcs;
                _token = token;
            }

            public async UniTask ExecuteAsync()
            {
                if (_token.IsCancellationRequested)
                {
                    _tcs.TrySetCanceled();
                    return;
                }

                try
                {
                    var r = await _request.ExecuteAsync(_token);
                    _tcs.TrySetResult(r);
                }
                catch (OperationCanceledException)
                {
                    _tcs.TrySetCanceled();
                }
                catch (Exception e)
                {
                    _tcs.TrySetException(e);
                }
            }
        }

        readonly Queue<IQueuedRequest> _queue = new();
        bool _processing;

        public UniTask<T> Enqueue<T>(IRequest<T> req, CancellationToken token)
        {
            var tcs = new UniTaskCompletionSource<T>();
            lock (_queue) _queue.Enqueue(new QueuedRequest<T>(req, tcs, token));
            _ = PumpLoop();
            return tcs.Task;
        }

        async UniTask PumpLoop()
        {
            if (_processing) return;
            _processing = true;

            while (true)
            {
                IQueuedRequest job;
                lock (_queue)
                {
                    if (_queue.Count == 0)
                    {
                        _processing = false; 
                        return;
                    }
                    job = _queue.Dequeue();
                }

                try
                {
                    await job.ExecuteAsync();
                }
                catch (Exception e)
                {
                    Debug.LogError($"RequestQueue swallowed: {e}");
                }
            }
        }

        public void Dispose()
        {
            lock (_queue) 
                _queue.Clear();
        }
    }
}
