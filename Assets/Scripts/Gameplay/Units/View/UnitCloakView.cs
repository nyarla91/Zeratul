using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitCloakView : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private SpriteRenderer _originalSpriteRenderer;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void Update()
        {
            _spriteRenderer.enabled = _unit.Visibility.IsCloaked && ! _unit.IsVisibleToPlayer;
            _spriteRenderer.sprite = _originalSpriteRenderer.sprite;
        }
    }
}