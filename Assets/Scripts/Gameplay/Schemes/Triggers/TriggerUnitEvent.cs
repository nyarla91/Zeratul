using Extentions;
using Gameplay.Data.Validator;
using Gameplay.Schemes.Values.Variables;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Triggers
{
    public abstract class TriggerUnitEvent : SchemeTrigger
    {
        [SerializeField] private VariableUnit _out;

        protected SchemeVariable<Unit> Out => _out;

        [Inject] private UnitPool UnitPool { get; set; }
        
        private void Awake()
        {
            UnitPool.UnitAdded += Subscribe;
        }

        protected abstract void Subscribe(Unit unit);

        protected void OutAndTrigger(Unit unit)
        {
            _out?.Set(unit);
            Trigger();
        }
    }
}