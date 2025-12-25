using Gameplay.Player;
using Gameplay.Units;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class UnitInfoPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _hitPoints;
        [SerializeField] private TMP_Text _shieldPoints;
        [SerializeField] private TMP_Text _energyPoints;

        public Unit CurrentUnit { get; private set; }

        [Inject] private PlayerSelection PlayerSelection { get; set; }

        private void Update()
        {
            CurrentUnit = PlayerSelection.SelectedUnits.Length == 1 ? PlayerSelection.SelectedUnits[0] : null;
            if ( ! CurrentUnit)
            {
                _canvasGroup.alpha = 0;
                return;
            }
            _canvasGroup.alpha = 1;

            _name.text = CurrentUnit.Type.DisplayName;
            UpdateStat(_hitPoints, true, CurrentUnit.Life.HitPoints, CurrentUnit.Life.MaxHitPoints);
            UpdateStat(_shieldPoints, CurrentUnit.Life.HasShieldPoints, CurrentUnit.Life.ShieldPoints, CurrentUnit.Life.MaxShieldPoints);
            UpdateStat(_energyPoints, CurrentUnit.Abilities.HasEnergyPoints, CurrentUnit.Abilities.EnergyPoints, CurrentUnit.Abilities.MaxEnergyPoints);
        }

        private void UpdateStat(TMP_Text stat, bool show, int current, int max)
        {
            stat.text = show ? $"{current} / {max}" : "";
        }
    }
}