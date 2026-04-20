using Gameplay.Data.Validator;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Unit = Gameplay.Units.Unit;

namespace Gameplay.Pickups
{
    public abstract class Pickup : MonoBehaviour
    {
        [SerializeField] private UnitValidatorGroup _interactionValidator;

        private void Awake()
        {
            this.OnTriggerEnter2DAsObservable()
                .Subscribe(TryPickUp);
        }

        private void TryPickUp(Collider2D other)
        {
            Debug.Log(other);
            Unit unit = other.GetComponentInParent<Unit>();
            if ( ! unit)
                return;
            Debug.Log(unit);
            if (_interactionValidator.IsInvalid(unit, unit))
                return;
            Debug.Log(unit);
            OnPickup(unit);
            Destroy(gameObject);
        }

        protected abstract void OnPickup(Unit picker);
    }
}