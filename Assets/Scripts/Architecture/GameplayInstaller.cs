using Gameplay;
using Gameplay.Player;
using Source.Extentions;
using UnityEngine;

namespace Architecture
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField]
        private PlayerSelection _playerSelection;
        
        [SerializeField]
        private PlayerOwnership _playerOwnership;
        
        public override void InstallBindings()
        {
            BindFromInstance(_playerSelection);
            BindFromInstance(_playerOwnership);
            Debug.Log(_playerSelection);
        }
    }
}