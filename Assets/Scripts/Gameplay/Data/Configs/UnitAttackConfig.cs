using Gameplay.Data.Orders;
using Gameplay.Data.Validator;
using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Unit Attack Config", order = 0)]
    public class UnitAttackConfig : ScriptableObject
    {
        [SerializeField] private float _deltaAngleTolerance;

        public float DeltaAngleTolerance => _deltaAngleTolerance;
    }
}