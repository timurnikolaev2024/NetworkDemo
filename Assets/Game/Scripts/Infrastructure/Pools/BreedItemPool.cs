using Game.UI;
using UnityEngine;
using Zenject;

namespace Game.Infrastructure
{
    public class BreedItemPool : MonoMemoryPool<Transform, BreedItemView>
    {
        protected override void Reinitialize(Transform parent, BreedItemView item)
        {
            item.transform.SetParent(parent, false);
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(BreedItemView item)
        {
            item.gameObject.SetActive(false);
        }
    }
}