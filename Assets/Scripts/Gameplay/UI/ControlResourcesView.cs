using Gameplay.Player;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class ControlResourcesView : MonoBehaviour
    {
        [SerializeField] private Image[] _slots;
        [SerializeField] private TMP_Text _extraReserve;
        [SerializeField] private Sprite _occupiedSprite;
        [SerializeField] private Sprite _availableSprite;
        [SerializeField] private Sprite _availableReserveSprite;
        
        [Inject] private PlayerControlResources ControlResources { get; set; }

        private void Awake()
        {
            this.UpdateAsObservable()
                .Subscribe(_ => UpdateSlots());
        }

        private void UpdateSlots()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (i >= ControlResources.Slots)
                {
                    _slots[i].gameObject.SetActive(false);
                    continue;
                }
                _slots[i].gameObject.SetActive(true);

                if (i < ControlResources.OccupiedSlots)
                {
                    _slots[i].sprite = _occupiedSprite;
                    continue;
                }
                if (i < ControlResources.OccupiedSlots + ControlResources.Reserve)
                {
                    _slots[i].sprite = _availableReserveSprite;
                    continue;
                }
                _slots[i].sprite = _availableSprite;
            }
            _extraReserve.gameObject.SetActive(ControlResources.ExtraReserve > 0);
            _extraReserve.text = $"+{ControlResources.ExtraReserve}";
        }
    }
}