using UnityEngine;

namespace Gameplay.Data.Configs
{
    [CreateAssetMenu(menuName = "Gameplay Data/Configs/Sprite Layering Config", order = 0)]
    public class SpriteLayeringConfig : ScriptableObject
    {
        [SerializeField] private LayerMask _spriteOverlatMask;
        [SerializeField] private int _unitBaseOrder;
        [SerializeField] private int _airUnitOrderBonus;
        [SerializeField] private int _shadowOrder;
        [SerializeField] private int _overlayDeltaOrderMultiplier;
        [SerializeField] private float _verticalZScale;

        public int UnitBaseOrder => _unitBaseOrder;
        public int AirUnitOrderBonus => _airUnitOrderBonus;
        public int ShadowOrder => _shadowOrder;
        public int OverlayDeltaOrderMultiplier => _overlayDeltaOrderMultiplier;
        public float VerticalZScale => _verticalZScale;
    }
}