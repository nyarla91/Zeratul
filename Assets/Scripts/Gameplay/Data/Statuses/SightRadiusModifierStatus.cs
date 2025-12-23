using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Sight Radius Modifier", order = 0)]
    public class SightRadiusModifierStatus : ModifierStatusType
    {
        protected override Modifier GetTargetModifier(Status status)
        {
            return status.Host.Sight.RadiusModifier;
        }
    }
}