using System;
using Extentions;
using UnityEngine;

namespace Gameplay.Data
{
    [Serializable]
    [CreateAssetMenu(menuName = "Gameplay Data/Graphics/Unit Sprite Map", order = 0)]
    public class UnitSpriteMap : ScriptableObject
    {
        private const int Directions = 8;

        private const float AngleStep = 360 / Directions;
        
        [SerializeField] private Sprite[] _spritePerDirection;

        public Sprite GetSpriteForAngle(float angle)
        {
            angle = angle.Snap(AngleStep);
            int directionIndex = Mathf.RoundToInt(angle / AngleStep).RepeatIndex(Directions);
            return _spritePerDirection[directionIndex];
        }
    }
}