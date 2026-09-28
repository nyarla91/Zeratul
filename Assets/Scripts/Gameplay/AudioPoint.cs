using _Core;
using Gameplay.Vision;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class AudioPoint : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _visibleVolume;
        [SerializeField] private float _notVisibleVolume;
        [SerializeField] private AnimationCurve _distanceToCameraMultiplier;
        [SerializeField] [Range(0, 1)] private float _panAmplitude;
        [SerializeField] private float _panMaxDistance;
        [SerializeField] private float _fadeSpeed;
        
        [Inject] private VisionMap VisionMap { get; set; }

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if ( ! VisionMap.IsPointSimulated(transform.position))
            {
                _audioSource.volume = 0;
                return;
            }
            float targetVolume = VisionMap.IsPointVisibleBy(transform.position, Owner.Player) ? _visibleVolume : _notVisibleVolume;
            float distanceToCamera = Vector3.Distance(_mainCamera.transform.position.WithZ(0), transform.position);
            targetVolume *= _distanceToCameraMultiplier.Evaluate(distanceToCamera);
            
            _audioSource.volume = Mathf.Lerp(_audioSource.volume, targetVolume, Time.deltaTime * _fadeSpeed);
            
            float panDistance = transform.position.x - _mainCamera.transform.position.x;
            float pan = Mathf.Clamp(panDistance / _panMaxDistance, -1, 1) * _panAmplitude;
            _audioSource.panStereo = pan;
        }
    }
}