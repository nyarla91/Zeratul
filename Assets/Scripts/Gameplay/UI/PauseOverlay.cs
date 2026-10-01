using UniRx;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Gameplay.UI
{
    public class PauseOverlay : MonoBehaviour
    {
        [SerializeField] private Volume _volume;
        [SerializeField] private float _offWeight;
        [SerializeField] private float _onWeight;
        [SerializeField] private float _fadeSpeed;
        
        [Inject] private TacticalPause TacticalPause { get; set; }

        private void Awake()
        {
            TacticalPause.ObserveEveryValueChanged(t => t.IsPaused)
                .Where(paused => paused)
                .Subscribe(_ => _volume.weight = 1);
        }

        private void Update()
        {
            float targetWeight = TacticalPause.IsPaused ? _onWeight : _offWeight; 
            _volume.weight = Mathf.Lerp(_volume.weight, targetWeight, Time.deltaTime * _fadeSpeed);
        }
    }
}