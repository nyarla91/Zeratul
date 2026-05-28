using System.Linq;
using Extentions;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Units.View
{
    [ExecuteInEditMode]
    public class UnitSpawnPointRadius : MonoBehaviour
    {
        private static Vector3[] _ellipsePoints;

        private static Vector3[] EllipsePoints
        {
            get
            {
                if (_ellipsePoints != null)
                    return _ellipsePoints;
                int pointsCount = 48;
                _ellipsePoints = new Vector3[pointsCount + 1];
                for (int i = 0; i < _ellipsePoints.Length; i++)
                {
                    float angle = (float) 360 / pointsCount * i;
                    _ellipsePoints[i] = angle.DegreesToVector2() * Isometry.Scale;
                }
                return _ellipsePoints;
            }
        }
        
        [SerializeField] private UnitSpawnPoint _spawnPoint;

#if UNITY_EDITOR
        private void DrawEllipse(Color color, float scale)
        {
            Vector3[] points = EllipsePoints.Select(v => v * scale + transform.position).ToArray();
            for (int i = 1; i < points.Length; i++)
            {
                Gizmos.color = color;
                Gizmos.DrawLine(points[i - 1], points[i]);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_spawnPoint.UnitType == null)
                return;
            DrawEllipse(Color.white, _spawnPoint.UnitType.SightRadius);
        }

        private void OnDrawGizmos()
        {
            if (_spawnPoint.UnitType == null)
                return;
            Color color = Selection.Contains(gameObject)
                ? _spawnPoint.UnitType.EditorRadiusColor.WithA(1)
                : _spawnPoint.UnitType.EditorRadiusColor;
            DrawEllipse(color, _spawnPoint.UnitType.EditorRadius);
        }
#endif
    }
}