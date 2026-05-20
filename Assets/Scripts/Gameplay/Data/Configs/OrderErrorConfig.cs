using Localization;
using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Order Error Config")]
    public class OrderErrorConfig : ScriptableObject
    {
        [SerializeField] private Localizer _localizer;
        [SerializeField] private string _generic;
        [SerializeField] private string _cannotMove;
        [SerializeField] private string _cannotAttack;
        [SerializeField] private string _targetInvalid;
        [SerializeField] private string _cantTargetSelf;
        [SerializeField] private string _notEnoughEnergy;
        [SerializeField] private string _notReadyAbility;
        [SerializeField] private string _outOfRange;
        [SerializeField] private string _locked;
        [SerializeField] private string _mustBeUnit;
        [SerializeField] private string _passive;

        public string Generic => _localizer.Translate(_generic);
        public string CannotMove => _localizer.Translate(_cannotMove);
        public string CannotAttack => _localizer.Translate(_cannotAttack);
        public string TargetInvalid => _localizer.Translate(_targetInvalid);
        public string CantTargetSelf => _localizer.Translate(_cantTargetSelf);
        public string NotEnoughEnergy => _localizer.Translate(_notEnoughEnergy);
        public string NotReadyAbility => _localizer.Translate(_notReadyAbility);
        public string OutOfRange => _localizer.Translate(_outOfRange);
        public string Locked => _localizer.Translate(_locked);
        public string MustBeUnit => _localizer.Translate(_mustBeUnit);
        public string Passive => _localizer.Translate(_passive);
    }
}