using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Weapon", order = 0)]
    public class UnitWeaponType : ScriptableObject
    {
        [SerializeField] private int _baseDamage;
        [SerializeField] private int _cooldown;
        [SerializeField] private float _maxDistance;
        [SerializeField] private bool _autoAttack;
        
        public int BaseDamage => _baseDamage;
        public int Cooldown => _cooldown;
        public float MaxDistance => _maxDistance;
        public bool AutoAttack => _autoAttack;
    }
}