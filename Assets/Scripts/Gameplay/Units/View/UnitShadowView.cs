using System;
using UnityEngine;

namespace Gameplay.Units.View
{
    public class UnitShadowView : MonoBehaviour
    {
        [SerializeField] private Unit _unit;

        private void Start()
        {
            transform.localScale = _unit.Type.Graphics.ShadowSize * Vector3.one;
        }
    }
}