using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Attacked", order = 0)]
    public class AttackedValidator : UnitValidator
    {
        [SerializeField] private SOInjectPresenter _gameplayPresenter;
        [SerializeField] private float _maxFramesSinceTookDamage;
        
        [Inject] private GameTime GameTime { get; set; }

        public override bool IsValid(Unit actor, Unit target)
        {
            _gameplayPresenter.Inject(this);
            return target.HasLife && GameTime.Frame - target.Life.LastDamageFrame < _maxFramesSinceTookDamage;
        }
    }
}