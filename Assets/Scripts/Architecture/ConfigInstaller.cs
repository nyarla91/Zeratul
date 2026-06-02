using Gameplay.Data.Configs;
using Localization;
using UnityEngine;
using Zenject;

namespace Architecture
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
        [SerializeField] private LayersConfig _layers;
        [SerializeField] private VisionConfig _vision;
        [SerializeField] private Localizer _localizer;
        [SerializeField] private Settings.Settings _settings;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_orderError);
            Container.BindInstance(_pathfinding);
            Container.BindInstance(_playerControl);
            Container.BindInstance(_spriteLayering);
            Container.BindInstance(_textFormatting);
            Container.BindInstance(_unitAttack);
            Container.BindInstance(_unitMovement);
            Container.BindInstance(_layers);
            Container.BindInstance(_vision);
            Container.BindInstance(_localizer);
            Container.BindInterfacesTo<Settings.Settings>().FromInstance(Instantiate(_settings));
            
            Container.Inject(_localizer);
        }
    }
}