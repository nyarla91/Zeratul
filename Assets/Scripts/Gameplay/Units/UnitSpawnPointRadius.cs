using System;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Units
{
    [ExecuteInEditMode]
    public class UnitSpawnPointRadius : MonoBehaviour
    {
        [SerializeField] private UnitSpawnPoint _spawnPoint;
        [SerializeField] private Transform _circle;

        private void Update()
        {
            if ( ! Selection.Contains(gameObject))
            {
                _circle.gameObject.SetActive(false);
                return;
            }
            _circle.gameObject.SetActive(true);
            _circle.localScale = _spawnPoint.UnitType?.EditorRadius * Isometry.Scale ?? Vector2.zero;
        }
    }
}