using System;
using Gameplay.Player;
using UniRx;
using UnityEngine;
using Zenject;
using Unit = Gameplay.Units.Unit;

namespace Gameplay.UI
{
    public class UnitPortraitRow : MonoBehaviour
    {
        [SerializeField] private UnitPortrait[] _portraits;

        [Inject] private PlayerUnitRow UnitRow { get; set; }
        
        private void Awake()
        {
            UnitRow.ObserveEveryValueChanged(u => u.Slots)
                .Subscribe(UpdatePortraits);
        }

        private void UpdatePortraits(Unit[] units)
        {
            for (int i = 0; i < _portraits.Length; i++)
            {
                if (i >= units.Length)
                {
                    _portraits[i].gameObject.SetActive(false);
                    continue;
                }
                _portraits[i].gameObject.SetActive(true);
                _portraits[i].Set(units[i], i);
            }
        }
    }
    
}