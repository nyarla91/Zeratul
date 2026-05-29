using System;
using System.Text;
using Extentions;
using Gameplay.Data.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Units
{
    public class UnitSpawnPoint : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _defaultSprite;
        [SerializeField] private UnitType _unitType;
        [SerializeField] private UnitSpawnInfo _spawnInfo;

        [Inject] private ContainerInstantiator Instantiator { get; set; }

        public UnitType UnitType => _unitType;
        public UnitSpawnInfo SpawnInfo => _spawnInfo;

        public event Action<Unit> Spawned;

        public void OnSpawn(Unit spawnedUnit)
        {
            Spawned?.Invoke(spawnedUnit);
            Destroy(gameObject);
        }
        
        private void OnValidate()
        {
            _spriteRenderer.sprite = UnitType?.SpriteMap?.GetSprite("idle", 0, SpawnInfo.LookAngle) ?? _defaultSprite;
            _spriteRenderer.color = SpawnInfo.Owner switch
            {
                Owner.Player => Color.green,
                Owner.Ally => Color.cyan,
                Owner.Neutral => Color.yellow,
                Owner.Enemy => Color.red,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            StringBuilder name =  new();
            string ownerLabel = SpawnInfo.Owner switch
            {
                Owner.Player => "Player",
                Owner.Ally => "Ally",
                Owner.Neutral => "Neutral",
                Owner.Enemy => "Enemy",
                _ => throw new ArgumentOutOfRangeException()
            };
            name.Append($"{ownerLabel} - ");
            name.Append(UnitType?.DisplayName ?? "No unit");
            gameObject.name = name.ToString();
        }

        private void OnDrawGizmos()
        {
            PatrolWaypoint[] waypoints = _spawnInfo.PatrolPath.Waypoints;
            for (int i = 0; i < waypoints.Length; i++)
            {
                Vector2 position = transform.position + (Vector3) waypoints[i].RelativePoint;
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(position, Vector3.one * 0.1f);
                if (i == 0)
                    continue;
                Vector2 previousPosition = transform.position + (Vector3) waypoints[i - 1].RelativePoint;
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(previousPosition, position);
            }
        }
    }
}