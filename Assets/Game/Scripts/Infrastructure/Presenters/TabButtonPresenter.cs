using System;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Infrastructure
{
    public sealed class TabButtonPresenter : IDisposable
    {
        private readonly Button _button;
        private readonly TabsPresenter _tabs;
        private readonly TabId _tabId;

        public TabButtonPresenter(Button button, TabsPresenter tabs, TabId tabId)
        {
            _button = button;
            _tabs = tabs;
            _tabId = tabId;

            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _tabs.ShowTab(_tabId);
        }

        public void Dispose()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    }
}