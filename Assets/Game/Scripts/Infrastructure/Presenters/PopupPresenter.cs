using Game.UI;
using UnityEngine;

namespace Game.Infrastructure
{
    public sealed class PopupPresenter
    {
        readonly BreedPopupView _view;
        
        public PopupPresenter(BreedPopupView view)
        {
            _view = view;
        }

        public void Show(string title, string body)
        {
            _view.Show(title, body);
        }

        public void Hide()
        {
            _view.Hide();
        }
    }
}