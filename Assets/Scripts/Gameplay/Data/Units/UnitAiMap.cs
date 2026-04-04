using System;
using System.Collections.Generic;
using Gameplay.AI;
using Gameplay.Data.Orders;
using Gameplay.Data.Validator;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Units
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit/Unit AI Map", order = 0)]
    public class UnitAiMap : ScriptableObject
    {
        [SerializeField] private AiOrder[] _aiOrders;

        public Order GetBestOrder(Unit agent, HashSet<Unit> surroundings)
        {
            Order result = null;
            float bestWorth = float.MinValue;
            foreach (AiOrder aiOrder in _aiOrders)
            {
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
            [SerializeField] private OrderType _order;
            [SerializeField] private UnitValidatorGroup _agentValidator;
            [SerializeField] private UnitValidatorGroup _targetValidator;
            [SerializeField] private AiUnitTargetEvaluatorGroup _evaluators;

            public Order GetOrder(Unit agent, HashSet<Unit> surroundings, out float worth)
            {
                worth = 0;
                if (IsAgentInvalid(agent))
                    return null;
                
                Order result = null;
                foreach (Unit target in surroundings)
                {
                    OrderTarget orderTarget = new(default, target);
                    if ( ! _order.IsTargetValid(agent, orderTarget, out _))
                        continue;
                    if (_targetValidator.IsInvalid(agent, target))
                        continue;

                    float newWorth = _evaluators.EvaluteTargetWorth(agent, target);
                    if (newWorth < worth)
                        continue;
                    
                    worth = newWorth;
                    result = new Order(_order, agent, orderTarget);
                }

                return result;
            }

            private bool IsAgentInvalid(Unit agent)
            {
                if ( ! _order.IsActorValid(agent, out _))
                    return true;
                if (_agentValidator.IsInvalid(agent, agent))
                    return true;
                return false;
            }
        }
    }
}