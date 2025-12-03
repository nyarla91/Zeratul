using System;
using UnityEngine;

namespace Extentions
{
    public static class EventExtentions
    {
        public static T GetNullableValue<T>(this Func<T> func)
        {
            if (func == null)
                throw new UnassignedReferenceException($"Value of {typeof(T)} type is never subscribed to");
            return func.Invoke();
        }
    }
}