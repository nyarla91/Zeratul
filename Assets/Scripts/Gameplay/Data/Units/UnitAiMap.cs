using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Data.AiEvaluators;
using Gameplay.Data.Orders;
using Gameplay.Data.Validator;
using Gameplay.Units;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Data.Units
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit/Unit AI Map", order = 0)]
    public class UnitAiMap : ScriptableObject
    {
        [SerializeField] private AiOrder[] _aiOrders;

        public Order GetBestOrder(Unit agent, HashSet<Unit> surroundings)
        {
            Order result = null;
            float bestWorth = 0;
            foreach (AiOrder aiOrder in _aiOrders)
            {
                if ( ! agent.Type.AvailableOrders.Contains(aiOrder.OrderType))
                    continue;
                Order newOrder = aiOrder.GetOrder(agent, surroundings, out float worth);
                if (worth < bestWorth)
                    continue;
                bestWorth = worth;
                result = newOrder;
            }
            return result;
        } 
        
        [Serializable]
        private struct AiOrder
        {
            [FormerlySerializedAs("_order")] [SerializeField] private OrderType _orderType;
            [SerializeField] private UnitValidatorGroup _agentValidator;
            [SerializeField] private UnitValidatorGroup _targetValidator;
            [SerializeField] private AiUnitTargetEvaluatorGroup _evaluators;
            
            public OrderType OrderType => _orderType;
            
            public Order GetOrder(Unit agent, HashSet<Unit> surroundings, out float worth)
            {
                worth = 0;
                if (IsAgentInvalid(agent))
                    return null;
                
                Order result = null;
                foreach (Unit target in surroundings)
                {
                    OrderTarget orderTarget = OrderTarget.FromUnit(target);
                    if ( ! _orderType.IsTargetValid(agent, orderTarget, out _))
                        continue;
                    if (_targetValidator.IsInvalid(agent, target))
                        continue;

                    float newWorth = _evaluators.EvaluteTargetWorth(agent, target);
                    if (newWorth < worth)
                        continue;
                    
                    worth = newWorth;
                    result = new Order(_orderType, agent, orderTarget);
                }

                return result;
            }

            private bool IsAgentInvalid(Unit agent)
            {
                if ( ! _orderType.IsActorValid(agent, out _))
                    return true;
                if (_agentValidator.IsInvalid(agent, agent))
                    return true;
                return false;
            }
        }
    }
}