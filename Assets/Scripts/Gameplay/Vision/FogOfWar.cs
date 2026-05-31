using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Extentions;
using Gameplay.Data.Configs;
using Gameplay.Player;
using Save.Data;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

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
        [SerializeField] private Color32 _enemyHighlightedColor;

        private bool _isBusy;
            
        private Sprite TargetSprite => _spriteRenderer.sprite;
        private Sprite EnemyTargetSprite => _enemySpriteRenderer.sprite;

        private bool _loaded;
        private FogOfWarCell[] _cells;
        private bool[] _repaintMask;
        private Color32[] _playerBuffer;
        private Color32[] _enemyBuffer;

        public FogOfWarCell[] Cells => _cells;

        [Inject] private VisionMap VisionMap { get; set; }
        [Inject] private PlayerSelection Selection { get; set; }

        private void Awake()
        {
            _isBusy = true;
            int width = _config.FogDimensions.x;
            int height = _config.FogDimensions.y;
            
            _cells = new FogOfWarCell[width * height];
            _repaintMask = new bool[width * height];
            _playerBuffer = new Color32[width * height];
            _enemyBuffer = new Color32[width * height];
            
            CreateTextureForRenderer(_spriteRenderer, width, height);
            CreateTextureForRenderer(_enemySpriteRenderer, width, height);
            
            this.FixedUpdateAsObservable()
                .Where(_ => ! _isBusy)
                .Subscribe(_ => Repaint());
        }

        private void CreateTextureForRenderer(SpriteRenderer spriteRenderer, int width, int height)
        {
            Texture2D texture = new(width, height, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Trilinear;
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

        private async void Start()
        {
            transform.localScale = _config.FogPixelScale * Vector3.one;
            
            if ( ! _loaded)
                Fill(FogOfWarCell.Hidden);
            await Paint();
            _isBusy = false;
        }

        public async void ReproduceFromSaveData(MapSaveSystem payload)
        {
            _cells = payload.cells;
            Paint();
        }

        public async UniTask Repaint()
        {
            _isBusy = true;

            HashSet<Bounds> playerBounds = VisionMap.PlayerBounds.ToHashSet();
            HashSet<VisionResult> playerVision = VisionMap.PlayerVisionSources.Select(v => v.Result).ToHashSet();
            HashSet<VisionResult> enemyVision = VisionMap.EnemyVisionSources.Select(v => v.Result).ToHashSet();
            VisionResult highlightedEnemyVision = Selection.IsUncontrollableSelected
                ? Selection.SelectedUnits[0].Sight?.VisionSource.Result ?? default
                : default;
                
            await UniTask.RunOnThreadPool(() =>
            {
                for (int i = 0; i < _cells.Length; i++)
                {
                    int x = i % _config.FogDimensions.x;
                    int y = i / _config.FogDimensions.x;
                    
                    FogOfWarCell previousCell = _cells[i];
                    
                    Vector2 point = new Vector2(x + 0.5f, y + 0.5f) * _config.FogPixelScale;
                    FogOfWarCell rawCell = GetCellForPoint(point, playerBounds, playerVision, enemyVision, highlightedEnemyVision);
                    _cells[i] = rawCell == FogOfWarCell.Hidden
                        ? _cells[i] == FogOfWarCell.Hidden ? FogOfWarCell.Hidden : FogOfWarCell.Scouted
                        : rawCell;

                    _repaintMask[i] = _cells[i] != previousCell;
                }
            });
            await Paint();
            _isBusy = false;
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
                        FogOfWarCell.EnemyHighlighted => _revealedColor,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                
                    _enemyBuffer[i] = cell switch
                    {
                        FogOfWarCell.Hidden => _revealedColor,
                        FogOfWarCell.Scouted => _revealedColor,
                        FogOfWarCell.Revealed => _revealedColor,
                        FogOfWarCell.EnemyRevealed => _enemyRevealedColor,
                        FogOfWarCell.EnemyHighlighted => _enemyHighlightedColor,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
            });
            
            TargetSprite.texture.SetPixelData(_playerBuffer, 0);
            TargetSprite.texture.Apply(false);
            EnemyTargetSprite.texture.SetPixelData(_enemyBuffer, 0);
            EnemyTargetSprite.texture.Apply(false);
        }

        private FogOfWarCell GetCellForPoint(Vector2 point, HashSet<Bounds> playerBounds,
            HashSet<VisionResult> playerVision, HashSet<VisionResult> enemyVision, VisionResult highlightedEnemyVision)
        {
            if ( ! IsPointInBounds(point, playerBounds) || ! IsPointVisible(point, playerVision))
                return FogOfWarCell.Hidden;
            if (IsPointVisible(point, new HashSet<VisionResult> {highlightedEnemyVision}))
                return FogOfWarCell.EnemyHighlighted;
            if (IsPointVisible(point, enemyVision))
                return FogOfWarCell.EnemyRevealed;
            return FogOfWarCell.Revealed;
        }

        private bool IsPointInBounds(Vector2 point, HashSet<Bounds> sourceBounds)
        {
            foreach (Bounds bounds in sourceBounds)
            {
                if (point.x >= bounds.min.x && point.x <= bounds.max.x 
                    && point.y >= bounds.min.y && point.y <= bounds.max.y)
                    return true;
            }
            return false;
        }

        private bool IsPointVisible(Vector2 point, HashSet<VisionResult> vision)
        {
            foreach (VisionResult visionSource in vision)
            {
                if (visionSource.IsPointVisible(point))
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