using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public sealed class BreedPopupView : MonoBehaviour
    {
        [SerializeField] GameObject _root;
        [SerializeField] Text _title;
        [SerializeField] Text _body;
        [SerializeField] Button _button;

        private void OnEnable()
        {
            _button.onClick.AddListener(Hide);
        }

        public void Show(string t, string b)
        {
            _title.text = t;
            _body.text  = b;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_root.GetComponent<RectTransform>());
            _root.SetActive(true);
        }

        public void Hide()
        {
            _root.SetActive(false);
        }
    }
}