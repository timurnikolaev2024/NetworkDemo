using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.UI;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Game.Infrastructure
{
    public sealed class WeatherPresenter : IInitializable, IDisposable
    {
        readonly WeatherView  _view;
        readonly RequestQueue _queue;
        CancellationTokenSource _cts;
        readonly Dictionary<string, Sprite> _iconCache = new();

        public WeatherPresenter(WeatherView view, RequestQueue queue)
        {
            _view = view; 
            _queue = queue;
        }

        public void Initialize()
        {
            _view.OnTabShown += StartLoop;
        }

        void StartLoop()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            _ = Loop(_cts.Token);
            _view.OnTabHidden += StopLoop;
        }
        
        async UniTaskVoid Loop(CancellationToken token)
        {
            var delay = TimeSpan.FromSeconds(5);
            while (!token.IsCancellationRequested)
            {
                try
                {
                    _view.ToggleLoader(true);
                    _view.ResetText();
                    _view.SetIcon(null);

                    var data = await _queue.Enqueue(new WeatherRequest(), token);
                    _view.ToggleLoader(false);
                    _view.SetText($"{data.Fahrenheit}F");

                    if (!_iconCache.TryGetValue(data.IconUrl, out var sprite))
                    {
                        sprite = await LoadIcon(data.IconUrl, token);
                        _iconCache[data.IconUrl] = sprite;
                    }
                    _view.SetIcon(sprite);

                    delay = TimeSpan.FromSeconds(30); // успех ↗ интервал
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Weather error: {e.Message}");
                    delay = TimeSpan.FromSeconds(Math.Min(delay.TotalSeconds * 2, 300));
                }
                await UniTask.Delay(delay, cancellationToken: token);
            }
        }
        
        async UniTask<Sprite> LoadIcon(string url, CancellationToken token)
        {
            using var req = UnityWebRequestTexture.GetTexture(url);
            req.SetRequestHeader("User-Agent", "Unity-Weather-Client");
            await req.SendWebRequest().ToUniTask(cancellationToken: token);
            if (req.result != UnityWebRequest.Result.Success) throw new Exception(req.error);
            var tex = DownloadHandlerTexture.GetContent(req);
            return Sprite.Create(tex, new Rect(0,0,tex.width,tex.height), Vector2.one*0.5f);
        }

        void StopLoop()
        {
            _view.OnTabHidden -= StopLoop;
            _cts?.Cancel();
        }
        public void Dispose()
        {
            _cts?.Cancel();
        }
    }
}