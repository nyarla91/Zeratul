using System;

namespace Extentions.Input
{
    public interface IBinding
    {
        bool IsHeld { get; }
        event Action Pressed;
        event Action Performed;
        event Action Released;
    }
}