using System;
using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Player Control Config", order = 0)]
    public class PlayerControlConfig : ScriptableObject
    {
        [SerializeField] private int _startingReserve;
        [SerializeField] private int _slots;

        public int StartingReserve => _startingReserve;
        public int Slots => _slots;

        private void OnValidate()
        {
            _startingReserve = Mathf.Max(0, _startingReserve);
            _slots = Mathf.Max(0, _slots);
        }
    }
}