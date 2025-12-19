using Extentions;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using PlayerInput = Gameplay.Player.PlayerInput;

namespace Gameplay
{
    public class CameraControl : MonoBehaviour
    { 
        [SerializeField] private Camera _camera;
        [SerializeField] private Range _zoomRange;
        [SerializeField] private float _zoomSpeed;
        [SerializeField] private float _dragSpeed;
        [SerializeField] private int _edgeTolerance;
        [SerializeField] private float _edgeMoveSpeed;
        
        [Inject] private PlayerInput PlayerInput { get; set; }

        private void Update()
        {
            if (PlayerInput.DragCameraBinding.IsHeld)
                DragCamera( - Mouse.current.delta.ReadValue());
            if (_edgeMoveSpeed > 0)
                EdgeMoveCamera();
            ZoomCamera(PlayerInput.ZoomDelta * Time.deltaTime);
        }

        private void EdgeMoveCamera()
        {
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
            transform.position +=  _edgeMoveSpeed * _camera.orthographicSize * Time.deltaTime * (Vector3) direction;
        }

        private void DragCamera(Vector2 screenDelta)
        {
            Vector3 worldDelta = _camera.ScreenToWorldPoint(screenDelta) - _camera.ScreenToWorldPoint(Vector3.zero);
            transform.position += worldDelta * _dragSpeed;
        }

        private void ZoomCamera(float zoomDelta)
        {
            _camera.orthographicSize +=  zoomDelta *  _zoomSpeed;
            _camera.orthographicSize = _zoomRange.Clamp(_camera.orthographicSize);
        }
    }
}