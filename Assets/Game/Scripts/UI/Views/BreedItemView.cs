using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class BreedItemView : MonoBehaviour
    {
        [SerializeField] Text _indexText;
        [SerializeField] Text _nameText;
        [SerializeField] Button _button;
        [SerializeField] GameObject _loader;

        private Action _onClick;
        
        public void Init(int index, string name, Action onClick)
        {
            _indexText.text = index.ToString();
            _nameText.text  = name;
            _onClick       = onClick;

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(OnClicked);

            ShowLoader(false);
        }

        void OnClicked()
        {
            _onClick?.Invoke();
        }

        public void ShowLoader(bool state)
        {
            _loader?.SetActive(state);
        }
    }
}