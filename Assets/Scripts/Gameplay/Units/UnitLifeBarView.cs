using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Units
{
    public class UnitLifeBarView : MonoBehaviour
    {
        [SerializeField] private UnitLife _model;
        [SerializeField] private RectTransform _hitPointsTransform;
        [SerializeField] private CanvasGroup _hitPointsGroup;
        [SerializeField] private RectMask2D _hitPointsMask;
        [SerializeField] private RectTransform _shieldPointsTransform;
        [SerializeField] private CanvasGroup _shieldsPointsGroup;
        [SerializeField] private RectMask2D _shieldsPointsMask;

        private void Update()
        {
            UpdateBar(_hitPointsGroup, _hitPointsMask, (float) _model.HitPoints / _model.MaxHitPoints);
            UpdateBar(_shieldsPointsGroup, _shieldsPointsMask, (float)_model.ShieldPoints / _model.MaxShieldPoints);
        }

        private void UpdateBar(CanvasGroup group, RectMask2D mask, float percent)
        {
            if (percent.Equals(1))
            {
                group.alpha = 0;
                return;
            }
            group.alpha = 1;
            float maxPadding = _hitPointsTransform.rect.width;
            float padding = (1 - percent) * maxPadding;
            mask.padding = new Vector4(0, 0, padding, 0);
        }
    }
}