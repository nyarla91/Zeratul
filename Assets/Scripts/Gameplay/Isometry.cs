using UnityEngine;

namespace Gameplay
{
    public static class Isometry
    {
        public const float VerticalScale = 0.5f;

        public static Vector2 Scale => new(1, VerticalScale);

        public static float Distance(Vector2 a, Vector2 b) => Vector2.Distance(Vector2.zero, (b - a) / Scale);
        
        public static float Magnitude(Vector2 vector) => Distance(Vector2.zero, vector);

        public static float DistanceTowards(float distance, float directionY) => distance * Mathf.Lerp(1, VerticalScale, Mathf.Abs(directionY));
    }
}