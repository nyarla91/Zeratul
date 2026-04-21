using Gameplay.Units;
using GameState;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Validator
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit Validator/Scenario Abilities Available", order = 0)]
    public class ScenarioAbilitiesAvailableValidator : UnitPropertyValidator
    {
        [SerializeField] private SOInjectPresenter _projectPresenter;
        
        [Inject] private ScenarioSession ScenarioSession { get; set; }
        
        protected override int GetUnitProperty(Unit unit)
        {
            _projectPresenter.Inject(this);
            return ScenarioSession.Current.AbilitiesAvailable;
        }
    }
}