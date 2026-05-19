using System;
using UnityEngine;

namespace Gameplay.Schemes.Triggers
{
    public abstract class SchemeTrigger : MonoBehaviour
    {
        public event Action Triggered;

        protected void Trigger()
        {
            Triggered?.Invoke();
        }
    }
}