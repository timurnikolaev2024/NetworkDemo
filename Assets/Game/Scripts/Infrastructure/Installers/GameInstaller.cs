using System.Collections.Generic;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Infrastructure
{
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField] private List<TabButtonConfig> _tabButtons;
        [SerializeField] private WeatherView _weatherView;
        [SerializeField] private BreedsView _breedsView;
        [SerializeField] private BreedPopupView _popupView;
        [SerializeField] private BreedItemView _prefabBreedItemView;
        [SerializeField] private Transform _root;

        public override void InstallBindings()
        {
            Container.Bind<RequestQueue>().AsSingle();

            Container.BindInterfacesAndSelfTo<WeatherView>().FromInstance(_weatherView);
            Container.BindInterfacesAndSelfTo<BreedsView>().FromInstance(_breedsView);
            Container.Bind<BreedPopupView>().FromInstance(_popupView);

            Container.Bind<TabsPresenter>().AsSingle();
            Container.BindInterfacesTo<WeatherPresenter>().AsSingle();
            Container.BindInterfacesTo<BreedsPresenter>().AsSingle();
            Container.Bind<PopupPresenter>().AsSingle().WithArguments(_popupView);

            Container.BindIFactory<Button, TabId, TabButtonPresenter>()
                .To<TabButtonPresenter>()
                .FromFactory<TabButtonFactory>();

            Container.BindInterfacesTo<TabButtonInitializer>()
                .AsSingle()
                .WithArguments(_tabButtons);
            
            Container.BindMemoryPool<BreedItemView, BreedItemPool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_prefabBreedItemView)
                .UnderTransform(_root);
        }
    }
}