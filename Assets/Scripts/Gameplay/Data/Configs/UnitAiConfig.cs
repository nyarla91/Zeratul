using Gameplay.Data.Orders;
using Gameplay.Data.Units;
using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/AI Config", order = 0)]
    public class UnitAiConfig : ScriptableObject
    {
        [SerializeField] private OrderType _moveOrder;
        [SerializeField] private float _timeBetweenThinking;
        [SerializeField] private float _damageForgiveTime;

        public OrderType MoveOrder => _moveOrder;
        public float TimeBetweenThinking => _timeBetweenThinking;
        public float DamageForgiveTime => _damageForgiveTime;
    }
}