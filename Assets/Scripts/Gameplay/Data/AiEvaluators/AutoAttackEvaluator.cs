using Extentions;
using Gameplay.Units;
using UnityEngine;
using static System.Single;

namespace Gameplay.Data.AiEvaluators
{
    [CreateAssetMenu(menuName = "Gameplay Data/AI Evaluator/Auto Attack", order = 0)]
    public class AutoAttackEvaluator : AiUnitTargetEvaluator
    {
        public override float EvaluteTargetWorth(Unit agent, Unit target)
        {
            if ( ! agent.CanAttack || target.Type.IsInvulnerable || target.Type.NonInteractable)
                return MinValue;
            float targetWorth = target.Type.AttackWorth.EvaluteTargetWorth(agent, target) / target.Life.MaxLifePoints * 100;
            return targetWorth / TimeToKill(agent, target);
        }

        private float TimeToKill(Unit agent, Unit target)
        {
            float totalTargetHitPoints = target.Life.LifePoints;
            float agentDPS = GetDPS(agent);
            float travelDistance = Isometry.Distance(agent.Position, target) - agent.Type.WeaponType.MaxDistance;
            return totalTargetHitPoints / agentDPS + TimeToReach(agent, target, travelDistance);
        }

        private float TimeToReach(Unit agent, Unit target, float travelDistance)
        {
            if (travelDistance <= 0)
                return 0;
            if ( ! agent.CanMove || agent.Movement.IsHoldingPosition)
                return MaxValue;
            float agentSpeed = agent.Movement.Speed;
            float targetRelativeSpeed = GetTargetRelativeSpeed(agent, target);
            float approachSpeed = agentSpeed - targetRelativeSpeed;
            if (approachSpeed <= 0)
                return MaxValue;
            return (travelDistance - agent.Type.WeaponType.MaxDistance) / approachSpeed;
        }

        private float GetTargetRelativeSpeed(Unit agent, Unit target)
        {
            if ( ! target.CanMove || target.Movement.Velocity.Equals(Vector2.zero))
                return 0;
            Vector2 directionTowardsTarget = agent.Position.DirectionTo(target.Position);
            float deltaAngle = Vector2.Angle(directionTowardsTarget, target.Movement.Velocity);
            float departureFactor = deltaAngle.Remap(0, 180, 1, -1);
            return departureFactor * target.Movement.Speed;
        }

        private float GetDPS(Unit unit)
        {
            float damage = unit.Type.WeaponType.BaseDamage;
            float framesBetweenAttacks = unit.Type.WeaponType.WindupTime + unit.Type.WeaponType.RecoveryTime;
            float attacksPerSecond = framesBetweenAttacks / 50;
            return damage * attacksPerSecond;
        }
    }
}