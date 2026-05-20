using Gameplay.Units;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;

namespace Gameplay.Schemes.Values.Variables
{
    public class VariableUnit : SchemeVariable<Unit>
    {
        [SerializeField] private UnitSpawnPoint _spawnPoint;

        protected override Unit DefaultValue => null;
        protected override string DisplayDefaultValue => _spawnPoint?.name;

        [Inject] private IGetUnitByIdService GetUnitByIdService { get; set; }

        public override string Save()
        {
            return JsonConvert.SerializeObject(value?.Id ?? -1);
        }

        public override void ReproduceFromSaveData(string json)
        {
            int unitId = JsonConvert.DeserializeObject<int>(json);
            value = GetUnitByIdService.GetUnitById(unitId);
        }

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