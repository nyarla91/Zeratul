using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Data.Statuses
{
    [CreateAssetMenu(menuName = "Gameplay Data/Statuses/Blank", order = 0)]
    public class BlankStatus : StatusType
    {
        public override void OnAdd(Status status) { }

        public override void OnUpdate(Status status) { }

        public override void OnRemove(Status status) { }
    }
}