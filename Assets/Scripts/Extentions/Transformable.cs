using System;
using UnityEngine;

namespace Source.Extentions
{
    public abstract class Transformable : MonoBehaviour
    {
        private Transform _transform;
        [Obsolete("Use cached property Transform instead")] public new Transform transform => base.transform;
        public Transform Transform => _transform ??= gameObject.transform;
    }
}