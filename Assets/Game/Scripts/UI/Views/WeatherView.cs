using System;
using Game.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class WeatherView : MonoBehaviour, ITabView
    {
        [SerializeField] Image _icon;
        [SerializeField] Text _label;
        [SerializeField] private TabId _id;
        [SerializeField] private GameObject _loader;

        public event Action OnTabShown;
        public event Action OnTabHidden;
        public TabId Id => _id;

        void OnEnable()
        {
            OnTabShown?.Invoke();
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        void OnDisable()
        {
            OnTabHidden?.Invoke();
        }

        public void SetText(string info)
        {
            _label.text = $"Сегодня — {info}F";
        }

        public void ResetText()
        {
            _label.text = string.Empty;
        }
        
        public void ToggleLoader(bool state)
        {
            _loader.SetActive(state);
        }
        
        public void SetIcon(Sprite sprite)
        {
            _icon.enabled = sprite != null;
            _icon.sprite = sprite;
        }
    }
}