using System;
using System.Collections.Generic;
using Game.Infrastructure;
using UnityEngine;
using Zenject;

namespace Game.UI
{
    public sealed class BreedsView : MonoBehaviour, ITabView
    {
        [SerializeField] private GameObject _loader;
        [SerializeField] private Transform _contentRoot;
        [SerializeField] private TabId _id;

        public TabId Id => _id;
        public event Action OnTabShown;
        public event Action OnTabHidden;
        private readonly List<BreedItemView> _items = new();
        [Inject] private BreedItemPool _pool;

        void OnEnable()
        {
            OnTabShown?.Invoke();
        }

        void OnDisable()
        {
            OnTabHidden?.Invoke();
        }

        public void ShowBreeds(IReadOnlyList<Breed> breeds, Action<Breed, BreedItemView> onClick)
        {
            for (int i = _items.Count; i < breeds.Count; i++)
            {
                var item = _pool.Spawn(_contentRoot);
                _items.Add(item);
            }

            for (int i = 0; i < breeds.Count; i++)
            {
                var breed = breeds[i];
                var item = _items[i];
                item.gameObject.SetActive(true);
                item.Init(i + 1, breed.Name, () => onClick(breed, item));
                item.transform.SetSiblingIndex(i);
            }

            for (int i = breeds.Count; i < _items.Count; i++)
            {
                _items[i].gameObject.SetActive(false);
            }
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ToggleLoader(bool state)
        {
            _loader.SetActive(state);
        }
        
        public void Clear()
        {
            foreach (var item in _items)
                _pool.Despawn(item);
            _items.Clear();
        }
    }
}