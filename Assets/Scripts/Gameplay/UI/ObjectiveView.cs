using System;
using _Core;
using Settings.Localization;
using TMPro;
using UniRx;
using UnityEngine;

namespace Gameplay.UI
{
    public class ObjectiveView : MonoBehaviour
    {
        [SerializeField] private Localizer _localizer;
        [SerializeField] private TMP_Text _line;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _completedColor;
        [SerializeField] private Color _failedColor;
        
        private Func<Objective> _objective;
        private IDisposable _observable;

        public Objective Objective => _objective.Invoke();
        public int Priority { get; private set; }
        
        public void Init(Func<Objective> objective, int priority)
        {
            if (_objective != null)
                return;
            _objective = objective;
            Priority = priority;
            _observable = Observable.EveryUpdate()
                .Subscribe(_ => UpdateView());
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
            _line.text = label + counter;

            Color color = Objective.Status switch
            {
                ObjectiveStatus.Active => _activeColor,
                ObjectiveStatus.Completed => _completedColor,
                ObjectiveStatus.Failed => _failedColor,
                _ => throw new ArgumentOutOfRangeException()
            };
            _line.color = color;
        }

        private void OnDestroy()
        {
            _observable.Dispose();
        }
    }
}