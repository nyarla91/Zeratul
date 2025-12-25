using System;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Speed Modifier", order = 0)]
    public class SpeedModifierStatus : ModifierStatusType
    {
        protected override Modifier GetTargetModifier(Status status)
        {
            return status.Host.Movement.SpeedModifier;
        }
    }
}