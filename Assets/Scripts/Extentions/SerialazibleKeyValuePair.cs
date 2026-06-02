using System;
using UnityEngine;

namespace Extentions
{
    [Serializable]
    public class SerialazibleKeyValuePair<TKey, TValue>
    {
        public TKey key;
        public TValue value;
    }
}