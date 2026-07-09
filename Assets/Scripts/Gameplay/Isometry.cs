using _Core;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay
{
    public static class Isometry
    {
        public const float VerticalScale = 0.5f;

        public static Vector2 Scale => new(1, VerticalScale);

        public static float Distance(Vector2 a, Vector2 b) => Vector2.Distance(Vector2.zero, (b - a) / Scale);

        public static float Distance(Vector2 a, Unit b) => Distance(a, b.Position) - b.Type.Size / 2;
        
        public static float DistanceTowards(float distance, float directionY) => distance * Mathf.Lerp(1, VerticalScale, Mathf.Abs(directionY));

        public static float Distance(Unit a, Unit b) => Distance(a.Position, b.Position) - (a.Type.Size + b.Type.Size) / 2;
        
        public static float Multiplier(float angle) => Mathf.Lerp(1, VerticalScale, angle.DegreesToVector2().y); 
    }
}