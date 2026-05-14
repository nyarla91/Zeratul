using UnityEngine;

namespace Gameplay.Data.Abilities
{
    [CreateAssetMenu(menuName = "Gameplay Data/Ability Cooldown Group", order = 0)]
    public class AbilityCooldownGroup : ScriptableObject
    {
        [SerializeField] private int _cooldown;

        public int Cooldown => _cooldown;
    }
}