using System;
using Gameplay.Data.Configs;
using Save.Data;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Vision
{
    public class FogOfWar : MonoBehaviour
    {
        [SerializeField] private VisionConfig _config;
        [SerializeField] private VisionMap _visionMap;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _pixelScale;
        [SerializeField] private Vector2Int _fogDimensions;
        [SerializeField] private Color _hiddenColor;
        [SerializeField] private Color _scoutedColor;
        [SerializeField] private Color _revealedColor;
        [SerializeField] private LayerMask _revealMask;

        private Sprite TargetSprite => _spriteRenderer.sprite;

        private bool _loaded;

        private void Start()
        {
            this.FixedUpdateAsObservable()
                .Sample(TimeSpan.FromSeconds(_config.RecalculationPeriod))
                .Subscribe(_ => RecalculateFog());
            
            transform.localScale = _pixelScale * Vector3.one;
            
            if (_loaded)
                return;
            
            for (int y = 0; y < _fogDimensions.y; y++)
            {
                for (int x = 0; x < _fogDimensions.x; x++)
                {
                    TargetSprite.texture.SetPixel(x, y, _hiddenColor);
                }
            }
            TargetSprite.texture.Apply();
        }

        public void ReproduceFromSaveData(MapSaveSystem saveSystem)
        {
            bool[,] scouted = saveSystem.scoutedFogOfWar;
            for (int y = 0; y < _fogDimensions.y && y < scouted.GetLength(1); y++)
            {
                for (int x = 0; x < _fogDimensions.x && x < scouted.GetLength(0); x++)
                {
                    Debug.Log($"{x} {y} {scouted[x, y]}");
                    Color color = scouted[x, y] ? _scoutedColor : _hiddenColor;
                    TargetSprite.texture.SetPixel(x, y, color);
                }
            }
            TargetSprite.texture.Apply();
            _loaded = true;
        }

        public bool[,] GetScouted()
        {
            bool[,] result = new bool[_fogDimensions.x, _fogDimensions.y];
            
            for (int y = 0; y < _fogDimensions.y; y++)
            {
                for (int x = 0; x < _fogDimensions.x; x++)
                {
                    result[x, y] = ! TargetSprite.texture.GetPixel(x, y).Equals(_hiddenColor);
                }
            }
            return result;
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
                        TargetSprite.texture.SetPixel(x, y, _revealedColor);
                    else if (TargetSprite.texture.GetPixel(x, y).Equals(_revealedColor))
                        TargetSprite.texture.SetPixel(x, y, _scoutedColor);
                }
            }
            TargetSprite.texture.Apply();
        }
    }
}