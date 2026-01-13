using System;

namespace Extentions.Input
{
    public interface IInputBindingReadonly
    {
        bool IsHeld { get; }
        event Action Pressed;
        event Action Performed;
        event Action Released;
    }
}