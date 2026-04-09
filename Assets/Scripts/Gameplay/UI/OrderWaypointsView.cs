using System.Collections.Generic;
using System.Linq;
using Extentions.Pause;
using Gameplay.Data.Orders;
using Gameplay.Player;
using Gameplay.Units;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Gameplay.UI
{
    public class OrderWaypointsView : MonoBehaviour
    {
        [SerializeField] private GameObject _waypointPrefab;

        private readonly List<OrderWaypoint> _waypoints = new();

        private HashSet<Unit> UnitsToDisplay => TacticalPause.IsPaused
            ? UnitPool.PlayerUnits.ToHashSet().Union(Selection.SelectedUnits).ToHashSet()
            : Selection.SelectedUnits.ToHashSet();
        
        [Inject] private UnitPool UnitPool { get; set; }
        [Inject] private PlayerSelection Selection { get; set; }
        [Inject] private GamePause GamePause { get; set; }
        [Inject] private TacticalPause TacticalPause { get; set; }

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
                orders.AddRange(unit.Orders.PendingOrders);
                Vector3 previousPoint = unit.Position;
                
                foreach (Order order in orders)
                {
                    Vector3 worldTo = order.Type.TargetRequirement == TargetRequirement.None
                        ? previousPoint
                        : (order.Target.Unit ? order.Target.Unit.Position : order.Target.Point);
                    GetIdleWaypoint().Draw(order.Type.Icon, previousPoint, worldTo);
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