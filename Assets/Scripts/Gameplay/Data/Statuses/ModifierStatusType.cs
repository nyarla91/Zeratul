using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    public abstract class ModifierStatusType : StatusType
    {
        [SerializeField] private int _priority;
        [SerializeField] private ModifierOperation _operation;
        [SerializeField] private float _fraction;
        
        private Func<float, float> Processor
        {
            get
            {
                return _operation switch
                {
                    ModifierOperation.Add => (v => v + _fraction),
                    ModifierOperation.Multiply => (v => v * _fraction),
                    _ =>  throw new IndexOutOfRangeException()
                };
            }
        }
        
        public override void OnAdd(Status status)
        {
            GetTargetModifier(status).AddEntry(new ModifierEntry(this, Processor, _priority));
        }

        public override void OnUpdate(Status status)
        {
            
        }

        public override void OnRemove(Status status)
        {
            GetTargetModifier(status).RemoveEntry(this);
        }
        
        protected abstract Modifier GetTargetModifier(Status status);

        private enum ModifierOperation
        {
            Add,
            Multiply
        }
    }
}