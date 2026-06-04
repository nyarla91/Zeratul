using _Core;
using UnityEngine;

namespace Gameplay.Visual
{
    public class RangeEllipse : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private int _vertices;
        
        public void Move(Vector2 position) => transform.position = position;

        public void Set(float radius, float thickness, Color color)
        {
            Vector3[] points = new Vector3[_vertices];
            for (int i = 0; i < _vertices; i++)
            {
                float angle = 360f /  _vertices * i;
                points[i] = angle.DegreesToVector2() * Isometry.Scale * radius;
            }
            _lineRenderer.positionCount = points.Length;
            _lineRenderer.SetPositions(points);
            _lineRenderer.colorGradient = color.ToGradient();
            _lineRenderer.widthMultiplier = thickness;
        }
        
        public void Show()
        {
            _lineRenderer.enabled = true;
        }

        public void Hide()
        {
            _lineRenderer.enabled = false;
        }

        public void Release()
        {
            gameObject.SetActive(false);
        }
    }
}