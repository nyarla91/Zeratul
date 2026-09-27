using System;
using UnityEngine;
using Zenject;

namespace BGM
{
    public class BgmTrigger : MonoBehaviour
    {
        [SerializeField] private BgmBehaviour _behaviour;

        [Inject] private BgmCycle BgmCycle { get; set; }

        private void Start()
        {
            switch (_behaviour)
            {
                case BgmBehaviour.PlayMainMenu:
                    BgmCycle.PlayMainMenu();
                    break;
                case BgmBehaviour.PlayGameplay:
                    BgmCycle.PlayerGameplay();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private enum BgmBehaviour
        {
            PlayMainMenu,
            PlayGameplay
        }
    }
}