using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Vision
{
    public class FogOfWar : MonoBehaviour
    {
        [SerializeField] private VisionMap _visionMap;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _pixelScale;
        [SerializeField] private Vector2Int _fogDimensions;
        [SerializeField] private Color _hiddenColor;
        [SerializeField] private Color _scoutedColor;
        [SerializeField] private Color _revealedColor;
        [SerializeField] private LayerMask _revealMask;

        private Sprite _targetSprite;

        private void Start()
        {
            _visionMap.RecalculationTimer.Expired += RecalculateFog;
            
            transform.localScale = _pixelScale * Vector3.one;
            _targetSprite = _spriteRenderer.sprite;
            
            for (int y = 0; y < _fogDimensions.y; y++)
            {
                for (int x = 0; x < _fogDimensions.x; x++)
                {
                    _targetSprite.texture.SetPixel(x, y, _hiddenColor);
                }
            }
            _targetSprite.texture.Apply();
        }

        private void Update()
        {
            if (Keyboard.current.pKey.wasPressedThisFrame)
                RecalculateFog();
            
        }

        private void RecalculateFog()
        {
            for (int y = 0; y < _fogDimensions.y; y++)
            {
                for (int x = 0; x < _fogDimensions.x; x++)
                {
                    Vector2 point = new Vector2(x + 0.5f, y + 0.5f) * _pixelScale;
                    bool revealed = Physics2D.OverlapPoint(point, _revealMask);
                    
                    if (revealed)
                        _targetSprite.texture.SetPixel(x, y, _revealedColor);
                    else if (_targetSprite.texture.GetPixel(x, y).Equals(_revealedColor))
                        _targetSprite.texture.SetPixel(x, y, _scoutedColor);
                }
            }
            _targetSprite.texture.Apply();
        }
    }
}