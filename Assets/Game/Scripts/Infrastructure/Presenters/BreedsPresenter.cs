using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.UI;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{

    public sealed class BreedsPresenter : IInitializable, IDisposable
    {
        readonly BreedsView _view;
        readonly RequestQueue _queue;
        readonly PopupPresenter _popup;
        CancellationTokenSource _cts;

        public BreedsPresenter(BreedsView v, RequestQueue q, PopupPresenter p)
        {
            _view=v;
            _queue=q;
            _popup=p;
        }
        public void Initialize()
        {
            _view.OnTabShown += Load;
        }

        void Load()
        {
            Cancel();
            _cts = new CancellationTokenSource();
            _view.ToggleLoader(true);
            _queue.Enqueue(new BreedsRequest(), _cts.Token)
                .ContinueWith(OnLoaded, _cts.Token).Forget();
            _view.OnTabHidden += Cancel;
        }
        
        void OnLoaded(IReadOnlyList<Breed> breeds)
        {
            if (_cts.IsCancellationRequested) return;
            _view.ToggleLoader(false);
            _view.ShowBreeds(breeds, OnClick);
        }
        
        void OnClick(Breed b, BreedItemView item)
        {
            CancelFetch();
            _cts = new CancellationTokenSource();
            item.ShowLoader(true);
            Fetch(b, item, _cts.Token).Forget();
        }
        
        async UniTaskVoid Fetch(Breed b, BreedItemView item, CancellationToken t)
        {
            try
            {
                var info = await _queue.Enqueue(new BreedInfoRequest(b.Id), t);
                _popup.Show(info.Name, info.Description);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Breed info failed: {e.Message}");
            }
            finally
            {
                if (!t.IsCancellationRequested) 
                    item.ShowLoader(false);
            }
        }
        
        void CancelFetch()
        {
            _cts?.Cancel();
        }

        void Cancel()
        {
            _view.OnTabHidden -= Cancel;
            _popup.Hide();
            CancelFetch();
            _view.Clear();
        }
        
        public void Dispose()
        {
            CancelFetch();
        }
    }
}