using Extentions;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitSpriteColorView : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _cloakedColor;
        
        private void Awake()
        {
            this.FixedUpdateAsObservable()
                .Subscribe(_ => UpdateColor());
        }

        private void UpdateColor()
        {
            _spriteRenderer.color = GetColorForUnit(_unit);
        }
        
        private Color GetColorForUnit(Unit unit)
        {
            if (unit.Alliance.IsFriendly(Owner.Player) && unit.Visibility.IsHidden)
                return _cloakedColor;
            if ( ! unit.Alliance.IsFriendly(Owner.Player) && unit.Visibility.IsCloaked)
                return _cloakedColor;
            return _defaultColor;
        }
    }
}