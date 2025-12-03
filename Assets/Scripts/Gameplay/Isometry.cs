using UnityEngine;

namespace Gameplay
{
    public static class Isometry
    {
        public const float VerticalScale = 0.5f;

        public static Vector2 Scale => new(1, VerticalScale);
    }
}