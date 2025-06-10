using System.Collections.Generic;
using Game.UI;
using Zenject;

namespace Game.Infrastructure
{
    public sealed class TabsPresenter
    {
        private readonly Dictionary<TabId, ITabView> _views = new();
        private ITabView _current;

        [Inject]
        public TabsPresenter(List<ITabView> views)
        {
            foreach (var view in views)
                _views[view.Id] = view;
        }

        public void ShowTab(TabId id)
        {
            if (_views.TryGetValue(id, out var next))
            {
                if (_current == next) return;

                _current?.Hide();
                _current = next;
                _current.Show();
            }
        }
    }
}