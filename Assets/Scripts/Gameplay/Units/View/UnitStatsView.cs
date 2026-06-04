using System;
using _Core;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Units.View
{
    public class UnitStatsView : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _hitPoints;
        [SerializeField] private Image _shieldPoints;
        [SerializeField] private Image _energyPoints;
        [SerializeField] private Image _status;
        [SerializeField] private Color _playerColor;
        [SerializeField] private Color _allyColor;
        [SerializeField] private Color _neutralColor;
        [SerializeField] private Color _enemyColor;

        private RectTransform _canvasRectTransform;
        
        private void Start()
        {
            float canvasSize = _unit.Type.Size;
            float canvasYOffset = _unit.Type.SpriteMap?.SpriteHeight ?? 0;
            _canvasRectTransform = _canvas.GetComponent<RectTransform>();
            _canvasRectTransform.localScale = new Vector3(1, 0.5f, 1) * canvasSize;
            _canvasRectTransform.localPosition = new Vector3(0, canvasYOffset);

            if (_unit.Type.IsInvulnerable)
                _shieldPoints.enabled = false;
            if (_unit.Type.IsInvulnerable || ! _unit.Life.HasShieldPoints)
                _shieldPoints.enabled = false;
            if ( ! _unit.Abilities.HasEnergyPoints)
                _energyPoints.enabled = false;
            
            UpdateOwnershipColor(_unit.Alliance.CurrentOwner);
            _unit.ObserveEveryValueChanged(u => u.Alliance.CurrentOwner)
                .Subscribe(UpdateOwnershipColor);
            
            _unit.ObserveEveryValueChanged(u => u.IsVisibleToPlayer)
                .Subscribe(v => _canvas.enabled = v);
            
            this.UpdateAsObservable()
                .Subscribe(_ => UpdateStats());
            this.UpdateAsObservable()
                .Subscribe(_ => UpdateStatus());
        }

        private void UpdateOwnershipColor(Owner owner)
        {
            _hitPoints.color = owner switch
            {
                Owner.Player => _playerColor,
                Owner.Ally => _allyColor,
                Owner.Neutral => _neutralColor,
                Owner.Enemy => _enemyColor,
                _ => throw new ArgumentOutOfRangeException(nameof(owner), owner, null)
            };
        }

        private void UpdateStats()
        {
            _hitPoints.fillAmount = _unit.Life?.HitPercent ?? 0;
            _shieldPoints.fillAmount = _unit.Life?.ShieldPercent ?? 0;
            _energyPoints.fillAmount = _unit.Abilities.EnergyPercent;
        }

        private void UpdateStatus()
        {
            IStatusInfo[] statuses = _unit.Statuses.StatusesInfo;
            IStatusInfo displayedStatus = null;
            int maxPriority = int.MinValue;
            foreach (IStatusInfo status in statuses)
            {
                if ( ! status.Type.ShowDuration || status.Type.ShowDurationPriority < maxPriority)
                    continue;
                displayedStatus = status;
                maxPriority = status.Type.ShowDurationPriority;
            }

            if (displayedStatus == null || displayedStatus.FramesLeft <= 2)
            {
                _status.fillAmount = 0;
                return;
            }

            int framesTotal = displayedStatus.RemovalFrame - displayedStatus.RestartFrame;
            float percent = (float) displayedStatus.FramesLeft / framesTotal;
            _status.fillAmount = percent;
        }
    }
}