using System;
using _Core.Pause;
using Gameplay.Map;
using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class CursorView : MonoBehaviour
    {
        [SerializeField] private PlayerCamera _playerCamera;
        [SerializeField] private CursorDefinition _default;
        [SerializeField] private CursorDefinition _targeting;
        [SerializeField] private CursorDefinition _targetingError;
        [SerializeField] private CursorDefinition _select;
        [SerializeField] private CursorDefinition _drag;
        [SerializeField] private CursorDefinition[] _edge;

        [Inject] private GamePause GamePause { get; set; }
        [Inject] private PlayerMouseTargeting MouseTargeting { get; set; }
        [Inject] private PlayerOrderTargeter OrderTargeter { get; set; }
        [Inject] private PlayerOrdersDispatcher OrdersDispatcher { get; set; }
        
        private void Update()
        {
            if (GamePause.IsPaused)
                SetCursor(_default);
            if (_playerCamera.IsDragging)
                SetCursor(_drag);
            else if (_playerCamera.EdgeMoveDirection != Vector2Int.zero)
                SetCursor(GetCursorForCameraDirection(_playerCamera.EdgeMoveDirection));
            else if (OrderTargeter.IsTargeting)
            {
                bool error = ! OrdersDispatcher.CanIssueWithTarget(OrderTargeter.CurrentOrder, OrderTargeter.CurrentTarget);
                SetCursor(error ? _targetingError : _targeting);
            }
            else if (MouseTargeting.Unit)
                SetCursor(_select);
            else
                SetCursor(_default);
        }

        private CursorDefinition GetCursorForCameraDirection(Vector2Int direction)
        {
            direction.x = Mathf.Clamp(direction.x, -1, 1);
            direction.y = Mathf.Clamp(direction.y, -1, 1);

            return (direction.x, direction.y) switch
            {
                (1, 0) => _edge[0],
                (1, 1) => _edge[1],
                (0, 1) => _edge[2],
                (-1, 1) => _edge[3],
                (-1, 0) => _edge[4],
                (-1, -1) => _edge[5],
                (0, -1) => _edge[6],
                (1, -1) => _edge[7],
                _ => _default
            };
        }

        private void SetCursor(CursorDefinition definition)
        {
            Cursor.SetCursor(definition.Texture, definition.Hotspot, CursorMode.Auto);
        }

        private void OnDestroy()
        {
            SetCursor(_default);
        }

        [Serializable]
        private struct CursorDefinition
        {
            [SerializeField] private Texture2D _texture;
            [SerializeField] private Vector2 _hotspot;

            public Texture2D Texture => _texture;
            public Vector2 Hotspot => _hotspot;
        }
    }
}