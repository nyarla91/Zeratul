using System;
using System.Linq;
using _Core;
using _Core.Pause;
using Gameplay.Data.Configs;
using Gameplay.Data.Orders;
using Gameplay.Units;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Unit = Gameplay.Units.Unit;

namespace Gameplay.Player
{
    public class PlayerMouseTargeting
    {
        private readonly GamePause _gamePause;
        private readonly LayersConfig _config;

        public Unit Unit { get; private set; }
        public Vector2 Point { get; private set; }
        
        public Unit OverrideUnit { get; set; }

        [Inject]
        public PlayerMouseTargeting(GamePause gamePause, LayersConfig config)
        {
            _gamePause = gamePause;
            _config = config;

            Observable.EveryFixedUpdate()
                .Subscribe(_ => UpdateTargets());
        }

        public OrderTarget GetTargetForRequirement(TargetRequirement requirement)
        {
            return requirement switch
            {
                TargetRequirement.None => default,
                TargetRequirement.Point => OrderTarget.FromPoint(Point),
                TargetRequirement.Unit => OrderTarget.FromUnit(Unit),
                TargetRequirement.PointOrUnit => new OrderTarget(Point, Unit),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void UpdateTargets()
        {
            if (_gamePause.IsPaused)
                return;

            if (OverrideUnit)
            {
                Unit = OverrideUnit;
                Point = OverrideUnit.Position;
                return;
            }
            
            Point = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            
            Collider2D[] overlap = Physics2D.OverlapPointAll(Point, _config.UnitInteractionMask);
            Unit[] units = overlap.Select(x => x.transform.GetComponentInParent<Unit>()).ClearNull();
            units = units.Where(u => u.IsInteractable && u.CanBeTargetedByPlayer).ToArray();

            Unit = units.Length == 0 ? null : units.MinElement(u => Isometry.Distance(Point, u.InteractionPosition));
        }
    }
}