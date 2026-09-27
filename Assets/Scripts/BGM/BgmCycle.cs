using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BGM
{
    public class BgmCycle : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _mainMenuClip;
        [SerializeField] private List<AudioClip> _gameplayClips;
        [SerializeField] private float _fadeInDuration;
        [SerializeField] private float _fadeOutDuration;

        private void Awake()
        {
            _gameplayClips = _gameplayClips.OrderBy(_ => Random.value).ToList();
            _audioSource.Play();
            _audioSource.Pause();
        }

        public void Pause()
        {
            _audioSource.DOKill();
            _audioSource.DOFade(0, _fadeOutDuration).onComplete += () =>
            {
                _audioSource.Pause();
            };
        }

        public void Resume()
        {
            _audioSource.DOComplete();
            _audioSource.UnPause();
            _audioSource.DOFade(1, _fadeInDuration);
        }

        public void PlayMainMenu()
        {
            SwitchClipTo(_mainMenuClip);
            RotateGameplayClips();
            Resume();
        }

        public void PlayerGameplay()
        {
            SwitchClipTo(_gameplayClips.First());
            Resume();
        }

        private void SwitchClipTo(AudioClip clip)
        {
            if (_audioSource.clip == clip)
                return;
            _audioSource.clip = clip;
            _audioSource.Play();
        }

        private void RotateGameplayClips()
        {
            AudioClip first = _gameplayClips.First();
            _gameplayClips.RemoveAt(0);
            _gameplayClips.Add(first);
        }

        private void Update()
        {
            if (_gameplayClips.Contains(_audioSource.clip) && !_audioSource.isPlaying)
            {
                RotateGameplayClips();
                PlayerGameplay();
            }
        }
    }
}