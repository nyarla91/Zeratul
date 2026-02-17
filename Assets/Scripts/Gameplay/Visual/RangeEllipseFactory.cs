using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay.Visual
{
    public class RangeEllipseFactory : MonoBehaviour
    {
        [SerializeField] private RangeEllipse _prefab;
        
        private readonly List<RangeEllipse> _pool = new();
        
        public RangeEllipse Get()
        {
            RangeEllipse available = _pool.FirstOrDefault(e => ! e.gameObject.activeSelf);
            if (available)
            {
                available.gameObject.SetActive(true);
                return available;
            }
            
            RangeEllipse newObject = Instantiate(_prefab.gameObject, transform).GetComponent<RangeEllipse>();
            _pool.Add(newObject);
            return newObject;
        }
    }
}