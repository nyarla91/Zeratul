using System;
using _Core;
using _Core.Pause;
using DG.Tweening;
using Settings.Localization;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class ObjectiveView : MonoBehaviour
    {
        [SerializeField] private Localizer _localizer;
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _line;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _completedColor;
        [SerializeField] private Color _failedColor;
        [SerializeField] private AudioClip _updatedClip;
        [SerializeField] private AudioClip _completedClip;
        [SerializeField] private AudioClip _failedClip;
        [SerializeField] private float _pitchAmplitude;
        [SerializeField] private float _volumeScale;
        
        private Func<Objective> _objective;
        private IDisposable _observable;

        public Objective Objective => _objective.Invoke();
        public int Priority { get; private set; }
        
        [Inject] private AudioSource AudioSource { get; set; }
        [Inject] private GamePause GamePause { get; set; }
        
        public void Init(Func<Objective> objective, int priority)
        {
            if (_objective != null)
                return;
            _objective = objective;
            Priority = priority;
            _observable = Observable.EveryUpdate()
                .Subscribe(_ => UpdateView());

            _objective.ObserveEveryValueChanged(o => o.Invoke()?.Counter ?? -1)
                .Subscribe(c => PingStatus(ObjectiveStatus.Active));
            
            _objective.ObserveEveryValueChanged(o => o.Invoke()?.Status ?? ObjectiveStatus.Failed)
                .Subscribe(PingStatus);
        }

        private void UpdateView()
        {
            if (Objective == null)
            {
                gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);
            string label = _localizer.Translate(Objective.Label);
            string counter = Objective.Goal > 0 ? $" ({Objective.Counter}/{Objective.Goal})" : "";
            _line.text = $"> {label} {counter}";

            Color color = Objective.Status switch
            {
                ObjectiveStatus.Active => _activeColor,
                ObjectiveStatus.Completed => _completedColor,
                ObjectiveStatus.Failed => _failedColor,
                _ => throw new ArgumentOutOfRangeException()
            };
            _line.color = color;
        }

        private void PingStatus(ObjectiveStatus status)
        {
            if (GamePause.IsPaused)
                return;
            
            Color color = status switch
            {
                ObjectiveStatus.Active => _activeColor,
                ObjectiveStatus.Completed => _completedColor.WithA(1),
                ObjectiveStatus.Failed => _failedColor.WithA(1),
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
            Ping(color);
            
            AudioClip clip = status switch {
                ObjectiveStatus.Active => _updatedClip,
                ObjectiveStatus.Completed => _completedClip,
                ObjectiveStatus.Failed => _failedClip,
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
            Debug.Log(clip);
            AudioSource.PlayPitchedOneShot(clip, _pitchAmplitude, _volumeScale);
        }

        private void Ping(Color color)
        {
            _background.DOKill();
            _background.color = color;
            _background.DOFade(0, 1.2f);
        }

        private void OnDestroy()
        {
            _observable.Dispose();
        }
    }
}