using System;
using _Core;
using _Core.Pause;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay.Units.View
{
    public class UnitGlobalSounds : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private AudioClip _becomeSelectedClip;
        [SerializeField] private AudioClip _becomeUnselectedClip;
        [SerializeField] private float _pitchAmplitude;

        [Inject] private AudioSource AudioSource { get; set; }
        [Inject] private GamePause GamePause { get; set; }
        
        private void Awake()
        {
            _unit.ObserveEveryValueChanged(u => u.IsSelected)
                .Subscribe(PlaySelection);
        }

        private void PlaySelection(bool isSelected)
        {
            if (GamePause.IsPaused)
                return;
            AudioSource.PlayPitchedOneShot(isSelected ? _becomeSelectedClip : _becomeUnselectedClip, _pitchAmplitude);
        }
    }
}