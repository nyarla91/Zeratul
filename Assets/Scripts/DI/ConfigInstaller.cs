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
            Container.Bind().FromInstance(_orderError);
            Container.Bind().FromInstance(_pathfinding);
            Container.Bind().FromInstance(_playerControl);
            Container.Bind().FromInstance(_spriteLayering);
            Container.Bind().FromInstance(_textFormatting);
            Container.Bind().FromInstance(_unitAttack);
            Container.Bind().FromInstance(_unitMovement);
            Container.Bind().FromInstance(_vision);
            Container.Bind().FromInstance(_localizer);
        }
    }
}