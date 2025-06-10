using Game.UI;
using UnityEngine.UI;
using Zenject;

namespace Game.Infrastructure
{
    public class TabButtonFactory : IFactory<Button, TabId, TabButtonPresenter>
    {
        private readonly DiContainer _container;

        public TabButtonFactory(DiContainer container)
        {
            _container = container;
        }

        public TabButtonPresenter Create(Button button, TabId id)
        {
            return _container.Instantiate<TabButtonPresenter>(
                new object[] { button, _container.Resolve<TabsPresenter>(), id });
        }
    }
}