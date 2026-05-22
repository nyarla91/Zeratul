using UnityEngine;

namespace Gameplay.Units.View
{
    [ExecuteInEditMode]
    public class UnitSpawnPointRadius : MonoBehaviour
    {
        [SerializeField] private UnitSpawnPoint _spawnPoint;
        [SerializeField] private Transform _circle;

        private void Update()
        {
            _circle.gameObject.SetActive(true);
            _circle.localScale = _spawnPoint.UnitType?.EditorRadius * Isometry.Scale ?? Vector2.zero;
        }
    }
}