using Gameplay.UI;
using UnityEngine;
using Zenject;
using Unit = Gameplay.Units.Unit;

namespace Gameplay.Pickups
{
    public class TutorialPickup : Pickup
    {
        [SerializeField] private int _tutorialIndex;
        
        [Inject] private TutorialWindow TutorialWindow { get; set; }
        
        protected override void OnPickup(Unit picker)
        {
            TutorialWindow.Show(_tutorialIndex);
        }
    }
}