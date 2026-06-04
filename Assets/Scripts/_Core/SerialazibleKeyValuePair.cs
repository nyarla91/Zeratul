using System;

namespace _Core
{
    [Serializable]
    public class SerialazibleKeyValuePair<TKey, TValue>
    {
        public TKey key;
        public TValue value;
    }
}