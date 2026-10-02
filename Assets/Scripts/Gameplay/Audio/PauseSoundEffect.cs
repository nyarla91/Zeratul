using BGM;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Gameplay.Audio
{
    public class PauseSoundEffect : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private string _parameter;
        [SerializeField] private float _defaultValue;
        [SerializeField] private float _pausedValue;
        [SerializeField] private float _fadeSpeed;

        private float _t;
        
        [Inject] private TacticalPause TacticalPause { get; set; }
        [Inject] private BgmCycle BgmCycle { get; set; }
        
        private void Update()
        {
            float targetT = TacticalPause.IsPaused ? 1 : 0;
            _t = Mathf.Lerp(_t, targetT, Time.deltaTime * _fadeSpeed);
            BgmCycle.AudioSource.pitch = Mathf.Lerp(_defaultValue, _pausedValue, _t);
        }

        private void OnDestroy()
        {
            BgmCycle.AudioSource.pitch = _defaultValue;
        }
    }
}