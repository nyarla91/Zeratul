using System.Collections.Generic;
using System.Linq;
using Extentions;
using Gameplay.Units;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Values.Variables
{
    public class VariableUnitGroup : SchemeVariable<HashSet<Unit>>
    {
        [SerializeField] private UnitSpawnPoint[] _spawnPoints;

        protected override HashSet<Unit> DefaultValue => null;
        protected override string DisplayDefaultValue => _spawnPoints
            .Enumerate(", ", "", p => p?.name);

        [Inject] private IGetUnitByIdService GetUnitByIdService { get; set; }

        public override string Save()
        {
            return JsonConvert.SerializeObject(value.Select(u => u.Id).ToHashSet());
        }

        public override void ReproduceFromSaveData(string json)
        {
            HashSet<int> unitsId = JsonConvert.DeserializeObject<HashSet<int>>(json);
            value = unitsId.Select(id => GetUnitByIdService.GetUnitById(id)).ToHashSet();
        }

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