using System.Collections.Generic;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Values.Variables
{
    public class VariableUnitGroup : SchemeVariable<HashSet<Unit>>
    {
        [SerializeField] private UnitSpawnPoint[] _spawnPoints;

        protected override HashSet<Unit> DefaultValue => null;

        protected override void Awake()
        {
            base.Awake();
            foreach (UnitSpawnPoint spawnPoint in _spawnPoints)
            {
                spawnPoint.Spawned += unit =>
                {
                    if (unit == null)
                        return;
                    value.Add(unit);
                };
            }
        }
    }
}