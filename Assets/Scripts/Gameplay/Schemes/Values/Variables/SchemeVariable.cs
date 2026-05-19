using System;

namespace Gameplay.Schemes.Values.Variables
{
    public abstract class SchemeVariable<T> : SchemeValue<T>
    {
        protected T value;

        protected abstract T DefaultValue { get; }
        
        public override T Value => value;

        public void Set(T value) => this.value = value;

        protected virtual void Awake()
        {
            value = DefaultValue;
        }
    }
}