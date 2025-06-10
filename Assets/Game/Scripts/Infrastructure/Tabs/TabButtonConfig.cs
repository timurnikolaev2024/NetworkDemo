using System;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Infrastructure
{
    [Serializable]
    public class TabButtonConfig
    {
        [SerializeField] private TabId _id;
        [SerializeField] private Button _button;
        
        public TabId Id => _id;
        public Button Button => _button;
    }
}