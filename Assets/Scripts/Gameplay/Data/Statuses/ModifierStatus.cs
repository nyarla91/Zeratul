using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Modifier", order = 0)]
    public class ModifierStatus : StatusType
    {
        [SerializeField] private ModifierType _type;
        [SerializeField] private ModifierOperation _operation;
        [SerializeField] private float _value;
        [SerializeField] private int _priority;
        
        private Func<float, float> Processor
        {
            get
            {
                return _operation switch
                {
                    ModifierOperation.Add => (v => v + _value),
                    ModifierOperation.Multiply => (v => v * _value),
                    _ =>  throw new IndexOutOfRangeException()
                };
            }
        }
        
        public override void OnAdd(Status status)
        {
            Modifier targetModifier = GetUnitModifier(status.Host, _type);
            targetModifier?.AddEntry(new ModifierEntry(status, Processor, _priority));
        }

        public override void OnUpdate(Status status)
        {
            
        }

        public override void OnRemove(Status status)
        {
            Modifier targetModifier = GetUnitModifier(status.Host, _type);
            targetModifier?.RemoveEntry(status);
        }

        private Modifier GetUnitModifier(Unit unit, ModifierType type)
        {
            return type switch
            {
                ModifierType.MoveSpeed => unit.Movement?.SpeedModifier,
                ModifierType.AttackSpeed => unit.Attack?.AttackSpeedModifier,
                ModifierType.Sight => unit.Sight?.RadiusModifier,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private enum ModifierOperation
        {
            Add,
            Multiply
        }

        private enum ModifierType
        {
            MoveSpeed,
            AttackSpeed,
            Sight,
        }
    }
}