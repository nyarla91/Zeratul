using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Weapon", order = 0)]
    public class UnitWeaponType : ScriptableObject
    {
        [SerializeField] private int _baseDamage;
        [SerializeField] private int _winduoTime;
        [SerializeField] private int _recoveryTime;
        [SerializeField] private float _maxDistance;
        [SerializeField] private bool _autoAttack;
        
        public int BaseDamage => _baseDamage;
        public int WinduoTime => _winduoTime;
        public int RecoveryTime => _recoveryTime;
        public float MaxDistance => _maxDistance;
        public bool AutoAttack => _autoAttack;
    }
}