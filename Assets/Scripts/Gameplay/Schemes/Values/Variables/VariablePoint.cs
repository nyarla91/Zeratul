using Extentions;
using Newtonsoft.Json;
using UnityEngine;

namespace Gameplay.Schemes.Values.Variables
{
    public class VariablePoint : SchemeVariable<Vector2>
    {
        protected override Vector2 DefaultValue => transform.position;
        protected override string DisplayDefaultValue => DefaultValue.ToString();

        public override string Save()
        {
            return JsonConvert.SerializeObject(SerializableVector2.FromVector2(value));
        }

        public override void ReproduceFromSaveData(string json)
        {
            value = JsonConvert.DeserializeObject<SerializableVector2>(json).ToVector2();
        }
    }
}