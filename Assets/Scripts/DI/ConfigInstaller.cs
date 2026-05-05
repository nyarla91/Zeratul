using Gameplay.Data.Configs;
using Localization;
using UnityEngine;
using Zenject;

namespace DI
{
    public class ConfigInstaller : MonoInstaller
    {
        [SerializeField] private OrderErrorConfig _orderError;
        [SerializeField] private PathfindingConfig _pathfinding;
        [SerializeField] private PlayerControlConfig _playerControl;
        [SerializeField] private SpriteLayeringConfig _spriteLayering;
        [SerializeField] private TextFormattingConfig _textFormatting;
        [SerializeField] private UnitAttackConfig _unitAttack;
        [SerializeField] private UnitMovementConfig _unitMovement;
        [SerializeField] private VisionConfig _vision;
        [SerializeField] private Localizer _localizer;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_orderError);
            Container.BindInstance(_pathfinding);
            Container.BindInstance(_playerControl);
            Container.BindInstance(_spriteLayering);
            Container.BindInstance(_textFormatting);
            Container.BindInstance(_unitAttack);
            Container.BindInstance(_unitMovement);
            Container.BindInstance(_vision);
            Container.BindInstance(_localizer);
        }
    }
}