using System;
using _Core;
using Gameplay.Data.Units;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitStepSounds : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private float _volumeScale;
        [SerializeField] private float _pitchAmplitude;

        private float _distanceTraveled;
        
        private UnitSoundMap SoundMap => _unit.Type.SoundMap;

        private void FixedUpdate()
        {
            if (!_unit.CanMove)
            {
                Destroy(this);
                return;
            }
            _distanceTraveled += (_unit.Movement.Velocity / Isometry.Scale).magnitude * Time.fixedDeltaTime;
            while (_distanceTraveled >= SoundMap.StepDistance)
            {
                _distanceTraveled -= SoundMap.StepDistance;
                _audioSource.PlayPitchedOneShot(SoundMap.StepClips.Random(), _pitchAmplitude, _volumeScale);
            }
        }
    }
}