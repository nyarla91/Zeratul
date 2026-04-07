using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Extentions
{
    [Serializable]
    public class InterfaceReference<TInterface> where TInterface : class
    {
        [SerializeField] private Object _object;

        public TInterface I
        {
            get
            {
                if ( ! _object)
                    return null;
                if (_object is TInterface)
                    return _object as TInterface;
                throw new ArgumentException($"Object of type {_object.GetType()} is not {typeof(TInterface)}");
            }
        }
    }
}