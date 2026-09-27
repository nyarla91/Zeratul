using System.Linq;
using Gameplay.Player;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class SelectAllUnitsButton : MonoBehaviour
    {
        [Inject] private PlayerSelection PlayerSelection { get; set; }
        [Inject] private UnitPool UnitPool { get; set; }

        public void SelectAll()
        {
            PlayerSelection.SelectUnits(UnitPool.PlayerUnits.ToArray());
        }
    }
}