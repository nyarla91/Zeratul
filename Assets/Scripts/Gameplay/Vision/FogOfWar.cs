using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Extentions;
using Gameplay.Data.Configs;
using Save.Data;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Vision
{
    public class FogOfWar : MonoBehaviour
    {
        [SerializeField] private VisionConfig _config;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteRenderer _enemySpriteRenderer;
        [SerializeField] private Color32 _hiddenColor;
        [SerializeField] private Color32 _scoutedColor;
        [SerializeField] private Color32 _revealedColor;
        [SerializeField] private Color32 _enemyRevealedColor;

        private Sprite TargetSprite => _spriteRenderer.sprite;
        private Sprite EnemyTargetSprite => _enemySpriteRenderer.sprite;

        private bool _loaded;
        private FogOfWarCell[] _cells;
        private bool[] _repaintMask;
        private Color32[] _playerBuffer;
        private Color32[] _enemyBuffer;

        public FogOfWarCell[] Cells => _cells;

        [Inject] private VisionMap VisionMap { get; set; }

        private void Awake()
        {
            int width = _config.FogDimensions.x;
            int height = _config.FogDimensions.y;
            
            _cells = new FogOfWarCell[width * height];
            _repaintMask = new bool[width * height];
            _playerBuffer = new Color32[width * height];
            _enemyBuffer = new Color32[width * height];
            
            CreateTextureForRenderer(_spriteRenderer, width, height);
            CreateTextureForRenderer(_enemySpriteRenderer, width, height);
        }

        private void CreateTextureForRenderer(SpriteRenderer spriteRenderer, int width, int height)
        {
            Texture2D texture = new(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;
            
            Color32[] clear = new Color32[width * height];
            for (int i = 0; i < clear.Length; i++)
                clear[i] = _hiddenColor;

            texture.SetPixelData(clear, 0);
            texture.Apply(false);
            
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, width, height),
                new Vector2(0, 0),
                1f
            );

            spriteRenderer.sprite = sprite;
        }

        private void Start()
        {
            transform.localScale = _config.FogPixelScale * Vector3.one;
            
            if ( ! _loaded)
                Fill(FogOfWarCell.Hidden);
            Paint();
        }

        public void ReproduceFromSaveData(MapSaveSystem payload)
        {
            _cells = payload.cells;
            Paint();
        }

        public async UniTask Recalculate()
        {
            await UniTask.RunOnThreadPool(() =>
            {
                for (int i = 0; i < _cells.Length; i++)
                {
                    int x = i % _config.FogDimensions.x;
                    int y = i / _config.FogDimensions.x;
                    
                    FogOfWarCell previousCell = _cells[i];
                    
                    Vector2 point = new Vector2(x + 0.5f, y + 0.5f) * _config.FogPixelScale;
                    bool revealed = IsPointRevealable(point) && VisionMap.IsPointVisibleBy(point, Owner.Player);

                    if (revealed)
                    {
                        _cells[i] = VisionMap.IsPointVisibleBy(point, Owner.Enemy) ? FogOfWarCell.EnemyRevealed : FogOfWarCell.Revealed;
                    }
                    else
                    {
                        _cells[i] = _cells[i] == FogOfWarCell.Hidden ? FogOfWarCell.Hidden : FogOfWarCell.Scouted;
                    }

                    _repaintMask[i] = _cells[i] != previousCell;
                }
            });
            Paint();
        }

        private async UniTask Paint(bool[] mask = null)
        {
            bool paintAll = mask == null;
            await UniTask.RunOnThreadPool(() =>
            {
                for (int i = 0; i < _cells.Length; i++)
                {
                    if ( ! paintAll && ! mask[i])
                        continue;
                    FogOfWarCell cell = Cells[i];
                
                    _playerBuffer[i] = cell switch
                    {
                        FogOfWarCell.Hidden => _hiddenColor,
                        FogOfWarCell.Scouted => _scoutedColor,
                        FogOfWarCell.Revealed => _revealedColor,
                        FogOfWarCell.EnemyRevealed => _revealedColor,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                
                    _enemyBuffer[i] = cell switch
                    {
                        FogOfWarCell.Hidden => _revealedColor,
                        FogOfWarCell.Scouted => _revealedColor,
                        FogOfWarCell.Revealed => _revealedColor,
                        FogOfWarCell.EnemyRevealed => _enemyRevealedColor,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
            });
            
            TargetSprite.texture.SetPixelData(_playerBuffer, 0);
            TargetSprite.texture.Apply(false);
            EnemyTargetSprite.texture.SetPixelData(_enemyBuffer, 0);
            EnemyTargetSprite.texture.Apply(false);
        }

        private bool IsPointRevealable(Vector2 point)
        {
            foreach (Bounds bounds in VisionMap.PlayerSimulationBounds)
            {
                if (bounds.Contains(point))
                    return true;
            }
            return false;
        }

        private void Fill(FogOfWarCell cell)
        {
            for (int i = 0; i < _cells.Length; i++)
            {
                _cells[i] = cell;
            }
        }
    }
}