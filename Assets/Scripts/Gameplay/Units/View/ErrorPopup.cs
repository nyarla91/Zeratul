using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class ErrorPopup : PoolElement<ErrorPopup>
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _fadeOutDuration;
        [SerializeField] private Color _startColor;
        [SerializeField] private Color _endColor;

        private float _endTimestamp;

        public void InitError(Vector3 headPosition, string errorMessage)
        {
            transform.position = headPosition;
            _text.text = errorMessage;
            _text.alpha = 1;
            _text.color = _startColor;

            _endTimestamp = Time.time + _fadeOutDuration;

            transform.DOMove(headPosition + Vector3.up * (_moveSpeed * _fadeOutDuration), _fadeOutDuration)
                .SetEase(Ease.Linear);
            _text.DOColor(_endColor, 0.4f)
                .SetEase(Ease.OutFlash);
            DOTween.To(() => _text.alpha, value => _text.alpha = value, 0f, _fadeOutDuration)
                .SetTarget(_text);
        }

        private void Update()
        {
            if ( ! IsSpawned)
                return;

            if (Time.time > _endTimestamp)
                Despawn();
        }

        public override void OnSpawn()
        {
            
        }

        protected override void OnDespawn()
        {
            transform.DOKill();
            _text.DOKill();
            DOTween.Kill(_text);
        }
    }
}
