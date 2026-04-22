using System;
using Gameplay.Data.Validator;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Unit = Gameplay.Units.Unit;

namespace Gameplay.Pickups
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private UnitValidatorGroup _interactionValidator;

        public event Action<Unit> PickedUp;
        
        private void Awake()
        {
            this.OnTriggerEnter2DAsObservable()
                .Subscribe(TryPickUp);
        }

        private void TryPickUp(Collider2D other)
        {
            Unit unit = other.GetComponentInParent<Unit>();
            if ( ! unit)
                return;
            if (_interactionValidator.IsInvalid(unit, unit))
                return;
            OnPickup(unit);
            PickedUp?.Invoke(unit);
            Destroy(gameObject);
        }

        protected virtual void OnPickup(Unit picker) { }
    }
}