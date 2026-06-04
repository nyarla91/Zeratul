using Gameplay.UI;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Values.Variables
{
    public class VariableObjective : SchemeVariable<_Core.Objective>
    {
        [SerializeField] private int _displayPriority;
        
        [Inject] private ObjectiveViewFactory ViewFactory { get; set; }
        
        protected override _Core.Objective DefaultValue => null;

        protected override string DisplayDefaultValue => $"({Key})";

        private void Start()
        {
            ViewFactory.InitView(() => Value, _displayPriority);
        }
    }
}