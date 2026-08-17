using System;
using _Core;
using _Core.Pause;
using DG.Tweening;
using Gameplay.Player;
using Gameplay.Units;
using Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using PlayerInput = Gameplay.Player.PlayerInput;
using Range = _Core.Range;

namespace Gameplay.Map
{
    public class PlayerCamera : MonoBehaviour
    { 
        [SerializeField] private BoxCollider2D _mapBounds;
        [SerializeField] private Camera _camera;
        [SerializeField] private Range _zoomRange;
        [SerializeField] private float _zoomSpeed;
        [SerializeField] private float _dragSpeed;
        [SerializeField] private int _edgeTolerance;
        [SerializeField] private float _edgeMoveSpeed;
        [SerializeField] private bool _disableEdgeMovementInEditor;
        [SerializeField] private float _moveDuration;

        public bool IsDragging { get; private set; }
        public Vector2Int EdgeMoveDirection { get; private set; }
        
        [Inject] private PlayerInput PlayerInput { get; set; }
        [Inject] private PlayerSelection PlayerSelection { get; set; }
        [Inject] private GamePause GamePause { get; set; }
        [Inject] private ISettingsReadService Settings { get; set; }

        private void Awake()
        {
            PlayerSelection.UnitSelectedTwice += MoveToUnit;
        }

        private void MoveToUnit(Unit unit)
        {
            MoveTo(unit.Position, false);
        }

        public void MoveTo(Vector2 position, bool immediately)
        {
            Vector3 target = position.WithZ(transform.position.z);
            transform.DOKill();
            if (immediately)
            {
                transform.position = target;
                return;
            }
            transform.DOMove(target, _moveDuration);
            BoundCamera();
        }
        
        private void Update()
        {
            if (GamePause.IsPaused)
                return;

            IsDragging = PlayerInput.DragCamera.IsHeld;
            if (IsDragging)
            {
                DragCamera( - Mouse.current.delta.ReadValue());
                EdgeMoveDirection = Vector2Int.zero;
                return;
            }
            if (_edgeMoveSpeed > 0)
                EdgeMoveCamera();
            ZoomCamera(PlayerInput.ZoomDelta * Time.deltaTime);
            BoundCamera();
        }

        private void EdgeMoveCamera()
        {
#if UNITY_EDITOR
            if (_disableEdgeMovementInEditor)
                return;
#endif
            transform.DOKill();
            Cursor.lockState = CursorLockMode.Confined;
            Vector2 direction = Vector2.zero;
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            if (mousePosition.x >= Screen.width - _edgeTolerance)
                direction.x = 1;
            else if (mousePosition.x <= _edgeTolerance)
                direction.x = -1;
            if (mousePosition.y >= Screen.height - _edgeTolerance)
                direction.y = 1;
            else if (mousePosition.y <= _edgeTolerance)
                direction.y = -1;
            Vector3 velocity = _edgeMoveSpeed * Settings.CameraMoveSpeed * direction;
            transform.position +=  _camera.orthographicSize * Time.deltaTime * velocity;
            EdgeMoveDirection = Vector2Int.RoundToInt(direction);
        }

        private void BoundCamera()
        {
            float x = Mathf.Clamp(transform.position.x, _mapBounds.bounds.min.x, _mapBounds.bounds.max.x);
            float y = Mathf.Clamp(transform.position.y, _mapBounds.bounds.min.y, _mapBounds.bounds.max.y);
            transform.position = new Vector3(x, y, transform.position.z);
        }

        private void DragCamera(Vector2 screenDelta)
        {
            transform.DOKill();
            Vector3 worldDelta = _camera.ScreenToWorldPoint(screenDelta) - _camera.ScreenToWorldPoint(Vector3.zero);
            transform.position += worldDelta * _dragSpeed * Settings.CameraDragSpeed;
        }

        private void ZoomCamera(float zoomDelta)
        {
            _camera.orthographicSize +=  zoomDelta *  _zoomSpeed;
            _camera.orthographicSize = _zoomRange.Clamp(_camera.orthographicSize);
        }
    }
}