using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Order Error Config")]
    public class OrderErrorConfig : ScriptableObject
    {
        [SerializeField] private string _generic;
        [SerializeField] private string _cannotMove;
        [SerializeField] private string _cannotAttack;
        [SerializeField] private string _targetInvalid;
        [SerializeField] private string _cantTargetSelf;
        [SerializeField] private string _notEnoughEnergy;
        [SerializeField] private string _notReadyAbility;
        [SerializeField] private string _outOfRange;
        [SerializeField] private string _locked;

        public string Generic => _generic;
        public string CannotMove => _cannotMove;
        public string CannotAttack => _cannotAttack;
        public string TargetInvalid => _targetInvalid;
        public string CantTargetSelf => _cantTargetSelf;
        public string NotEnoughEnergy => _notEnoughEnergy;
        public string NotReadyAbility => _notReadyAbility;
        public string OutOfRange => _outOfRange;
        public string Locked => _locked;
    }
}