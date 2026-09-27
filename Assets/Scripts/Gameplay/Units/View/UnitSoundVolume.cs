using System;
using _Core;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitSoundVolume : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _visibleVolume;
        [SerializeField] private float _notVisibleVolume;
        [SerializeField] private AnimationCurve _distanceToCameraMultiplier;
        [SerializeField] [Range(0, 1)] private float _panAmplitude;
        [SerializeField] private float _panMaxDistance;
        [SerializeField] private float _fadeSpeed;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!_unit.IsSimulated)
            {
                _audioSource.volume = 0;
                return;
            }
            float targetVolume = _unit.IsVisibleToPlayer ? _visibleVolume : _notVisibleVolume;
            float distanceToCamera = Vector3.Distance(_mainCamera.transform.position.WithZ(0), _unit.Position);
            targetVolume *= _distanceToCameraMultiplier.Evaluate(distanceToCamera);
            
            _audioSource.volume = Mathf.Lerp(_audioSource.volume, targetVolume, Time.deltaTime * _fadeSpeed);
            
            float panDistance = _unit.Position.x - _mainCamera.transform.position.x;
            float pan = Mathf.Clamp(panDistance / _panMaxDistance, -1, 1) * _panAmplitude;
            _audioSource.panStereo = pan;
        }
    }
}