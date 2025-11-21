using UnityEngine;

namespace Source.Extentions
{
    public class PublicLazyGetComponent<T> : LazyGetComponent<T> where T : Component
    {
        public new T Lazy => base.Lazy;
    }
}