using System;
using _Core;

namespace Gameplay.Data
{
    public interface IRadiusSource
    {
        public float Radius { get; }
    }
    
    [Serializable]
    public class ReferenceIRadiusSource : InterfaceReference<IRadiusSource> { }
}