using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Schemes.Values.Variables
{
    public class VariableUnit : SchemeVariable<Unit>
    {
        [SerializeField] private UnitSpawnPoint _spawnPoint;

        protected override Unit DefaultValue => null;

        protected override void Awake()
        {
            base.Awake();
            _spawnPoint.Spawned += unit =>
            {
                if (unit == null)
                    return;
                value = unit;
            };
        }
    }
}