using System.Collections.Generic;
using Game.UI;
using UnityEngine.UI;
using Zenject;

namespace Game.Infrastructure
{
    public sealed class TabButtonInitializer : IInitializable
    {
        private readonly List<TabButtonConfig> _configs;
        private readonly IFactory<Button, TabId, TabButtonPresenter> _factory;

        public TabButtonInitializer(
            List<TabButtonConfig> configs,
            IFactory<Button, TabId, TabButtonPresenter> factory)
        {
            _configs = configs;
            _factory = factory;
        }

        public void Initialize()
        {
            foreach (var config in _configs)
                _factory.Create(config.Button, config.Id);
        }
    }
}