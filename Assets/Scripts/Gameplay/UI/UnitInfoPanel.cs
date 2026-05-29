using System.Linq;
using Extentions;
using Gameplay.Data.Units;
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
        [SerializeField] private TMP_Text _tags;

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
            bool displayHitPoints = CurrentUnit.HasLife;
            bool displayShieldPoints = CurrentUnit.HasLife && CurrentUnit.Life.HasShieldPoints;
            bool displayEnergyPoints = CurrentUnit.Abilities.HasEnergyPoints;
            UpdateStat(_hitPoints, displayHitPoints, CurrentUnit.Life?.HitPoints ?? 0, CurrentUnit.Life?.MaxHitPoints ?? 0);
            UpdateStat(_shieldPoints, displayShieldPoints, CurrentUnit.Life?.ShieldPoints ?? 0, CurrentUnit.Life?.MaxShieldPoints ?? 0);
            UpdateStat(_energyPoints, displayEnergyPoints, CurrentUnit.Abilities.EnergyPoints, CurrentUnit.Abilities.MaxEnergyPoints);
            
            UnitTag[] unitTags = CurrentUnit.Type.Tags.Where(t => t.Display).ToArray();
            _tags.text = unitTags.Select(t => t.DisplayName).Enumerate();
        }

        private void UpdateStat(TMP_Text stat, bool display, int current, int max)
        {
            stat.text = display ? $"{current} / {max}" : "";
        }
    }
}