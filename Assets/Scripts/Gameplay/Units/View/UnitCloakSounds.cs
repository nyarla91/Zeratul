using System;
using _Core.Pause;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Units.View
{
    public class UnitCloakSounds : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _hiddenClip;
        [SerializeField] private AudioClip _revealedClip;
        [SerializeField] private float _playerVolume;
        [SerializeField] private float _otherVolume;
        
        [Inject] private GamePause GamePause { get; set; }
        
        private void Start()
        {
            _unit.ObserveEveryValueChanged(u => u.Visibility.IsHidden)
                .Subscribe(Play);
        }

        private void Play(bool isHidden)
        {
            if (GamePause.IsPaused)
                return;
            AudioClip clip = isHidden ? _hiddenClip : _revealedClip;
            float volumeScale = _unit.Alliance.OwnedByPlayer ? _playerVolume : _otherVolume;
            _audioSource.PlayOneShot(clip, volumeScale);
        }
    }
}