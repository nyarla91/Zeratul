using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitInteractionCollision : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private BoxCollider2D _collider;

        private void Start()
        {
            transform.localPosition = Vector3.up * (_unit.Type.SpriteMap?.SpriteHeight ?? 0);
            _collider.size = _unit.Type.InteractionColliderSize;
            _collider.offset = _unit.Type.InteractionColliderOffset;
        }
    }
}