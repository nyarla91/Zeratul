using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitMaterialView : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _cloakedMaterial;
        [SerializeField] private Material _detectedCloakedMaterial;

        private void Update()
        {
            _spriteRenderer.material = GetMaterial();
        }
        
        private Material GetMaterial()
        {
            if ( ! _unit.Visibility.IsCloaked)
                return _defaultMaterial;
            if (_unit.Visibility.IsDetected)
                return _detectedCloakedMaterial;
            return _cloakedMaterial;
        }
    }
}