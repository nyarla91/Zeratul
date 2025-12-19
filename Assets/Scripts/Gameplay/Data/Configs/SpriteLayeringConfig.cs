using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Sprite Layering Config", order = 0)]
    public class SpriteLayeringConfig : ScriptableObject
    {
        [SerializeField] private int _unitBaseOrder;
        [SerializeField] private int _airUnitOrderBonus;
        [SerializeField] private int _shadowOrder;
        [SerializeField] private float _verticalZScale;

        public int UnitBaseOrder => _unitBaseOrder;
        public int AirUnitOrderBonus => _airUnitOrderBonus;
        public int ShadowOrder => _shadowOrder;
        public float VerticalZScale => _verticalZScale;
    }
}