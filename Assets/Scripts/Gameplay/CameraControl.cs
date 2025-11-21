using Source.Extentions;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using PlayerInput = Gameplay.Player.PlayerInput;

namespace Gameplay
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.Camera _camera;
        
        [SerializeField]
        private Range _zoomRange;

        [SerializeField]
        private float _zoomSpeed;
        
        [Inject]
        private PlayerInput PlayerInput { get; set; }

        private void Update()
        {
            if (PlayerInput.DragCameraBinding.IsHeld)
                DragCamera( - Mouse.current.delta.ReadValue());
            ZoomCamera(PlayerInput.ZoomDelta * Time.deltaTime);
        }

        private void DragCamera(Vector2 screenDelta)
        {
            Vector3 worldDelta = _camera.ScreenToWorldPoint(screenDelta) - _camera.ScreenToWorldPoint(Vector3.zero);
            transform.position += worldDelta;
        }

        private void ZoomCamera(float zoomDelta)
        {
            _camera.orthographicSize +=  zoomDelta *  _zoomSpeed;
            _camera.orthographicSize = _zoomRange.Clamp(_camera.orthographicSize);
        }
    }
}