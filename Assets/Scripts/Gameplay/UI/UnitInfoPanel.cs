using System.Linq;
using System.Text;
using _Core;
using Gameplay.Data.Configs;
using Gameplay.Data.Units;
using Gameplay.Player;
using Gameplay.Units;
using Settings.Localization;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class UnitInfoPanel : MonoBehaviour
    {
        [SerializeField] private TextFormattingConfig _formatting;
        [SerializeField] private Localizer _localizer;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _stats;
        [SerializeField] private TMP_Text _tags;
        [SerializeField] private string _statsSpacing;

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
            
            _name.text = _localizer.Translate(CurrentUnit.Type.DisplayName);
            
            UnitTag[] unitTags = CurrentUnit.Type.Tags.Where(t => t.Display).ToArray();
            _tags.text = unitTags.Select(t => t.DisplayName).Enumerate();
            
            StringBuilder stats = new();
            if (CurrentUnit.HasLife && CurrentUnit.Life.HasShieldPoints)
            {
                stats.Append($"<sp>{CurrentUnit.Life.ShieldPoints} / {CurrentUnit.Life.MaxShieldPoints}</sp>");
                stats.Append(_statsSpacing);
            }
            if (CurrentUnit.HasLife)
            {
                stats.Append($"<hp>{CurrentUnit.Life.HitPoints} / {CurrentUnit.Life.MaxHitPoints}</hp>");
                stats.Append(_statsSpacing);
            }
            if (CurrentUnit.Abilities.HasEnergyPoints)
            {
                stats.Append($"<ep>{CurrentUnit.Abilities.EnergyPoints} / {CurrentUnit.Abilities.MaxEnergyPoints}</ep>");
                stats.Append(_statsSpacing);
            }
            _stats.text = _formatting.Format(stats.ToString().Trim());
        }
    }
}