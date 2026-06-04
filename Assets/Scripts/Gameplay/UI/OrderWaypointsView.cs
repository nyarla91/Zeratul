using System.Collections.Generic;
using System.Linq;
using _Core;
using _Core.Pause;
using Gameplay.Data.Orders;
using Gameplay.Player;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class OrderWaypointsView : MonoBehaviour
    {
        [SerializeField] private RectTransform _hud;
        [SerializeField] private GameObject _waypointPrefab;

        private readonly List<OrderWaypoint> _waypoints = new();
        private Camera _mainCamera;

        private HashSet<Unit> UnitsToDisplay => TacticalPause.IsPaused
            ? UnitPool.PlayerUnits.ToHashSet().Union(Selection.SelectedUnits).ToHashSet()
            : Selection.SelectedUnits.ToHashSet();
        
        [Inject] private UnitPool UnitPool { get; set; }
        [Inject] private PlayerSelection Selection { get; set; }
        [Inject] private GamePause GamePause { get; set; }
        [Inject] private TacticalPause TacticalPause { get; set; }

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            foreach (OrderWaypoint waypoint in _waypoints)
                waypoint.Hide();

            foreach (Unit unit in UnitsToDisplay)
            {
                if ( ! unit)
                    continue;
                List<Order> orders = new();
                if (unit.Orders.CurrentOrder != null)
                    orders.Add(unit.Orders.CurrentOrder);
                orders.AddRange(unit.Orders.OrdersQueue);
                Vector3 previousPoint = unit.Position;
                
                foreach (Order order in orders)
                {
                    Vector3 worldTo = order.Type.TargetRequirement == TargetRequirement.None
                        ? previousPoint
                        : (order.Target.Unit ? order.Target.Unit.Position : order.Target.Point);

                    Vector2 screenFrom = _mainCamera.WorldToScreenPoint(previousPoint);
                    screenFrom = screenFrom.ScreenToCanvasPoint(_hud);
                    Vector2 screenTo = _mainCamera.WorldToScreenPoint(worldTo);
                    screenTo = screenTo.ScreenToCanvasPoint(_hud);
                    
                    GetIdleWaypoint().Draw(order.Type.Icon, screenFrom, screenTo);
                    previousPoint = worldTo;
                }
            }
        }

        private OrderWaypoint GetIdleWaypoint()
        {
            OrderWaypoint result = _waypoints.FirstOrDefault(w =>  w.IsHidden);
            if (result)
                return result;

            result = Instantiate(_waypointPrefab, transform).GetComponent<OrderWaypoint>();
            _waypoints.Add(result);
            return result;
        }
    }
}