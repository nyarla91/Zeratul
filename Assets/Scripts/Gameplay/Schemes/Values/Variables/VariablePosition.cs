using UnityEngine;

namespace Gameplay.Schemes.Values.Variables
{
    public class VariablePosition : SchemeVariable<Vector2>
    {
        protected override Vector2 DefaultValue => transform.position;
    }
}