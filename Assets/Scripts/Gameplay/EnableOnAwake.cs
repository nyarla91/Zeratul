using System;
using UnityEngine;

namespace Gameplay
{
    public class EnableOnAwake : MonoBehaviour
    {
        [SerializeField] private GameObject[] _objects;

        private void Awake()
        {
            foreach (GameObject o in _objects)
            {
                o.SetActive(true);
            }
        }
    }
}