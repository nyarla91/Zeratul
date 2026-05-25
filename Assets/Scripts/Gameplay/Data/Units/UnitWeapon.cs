using Gameplay.Data.Effects;
using UnityEngine;

namespace Gameplay.Data.Units
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit/Unit Weapon", order = 0)]
    public class UnitWeaponType : ScriptableObject, IRadiusSource
    {
        [SerializeField] private int _baseDamage;
        [SerializeField] private EffectTargetingUnit[] _additionalEffects;
        [SerializeField] private int _windupTime;
        [SerializeField] private int _recoveryTime;
        [SerializeField] private float _maxDistance;
        [SerializeField] private bool _autoAttack;
        
        public int BaseDamage => _baseDamage;
        public EffectTargetingUnit[] AdditionalEffects => _additionalEffects;
        public int WindupTime => _windupTime;
        public int RecoveryTime => _recoveryTime;
        public float MaxDistance => _maxDistance;
        public bool AutoAttack => _autoAttack;
        public float Radius => MaxDistance;
    }
}