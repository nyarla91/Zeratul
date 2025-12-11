using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Extentions
{
    public static class VectorExtentions
    {
        private static Dictionary<Axis, Vector2> _axes2;
        public static Dictionary<Axis, Vector2> Axes2
        {
            get
            {
                if (_axes2 == null)
                {
                    _axes2 = new Dictionary<Axis, Vector2>();
                    _axes2.Add(Axis.X, Vector2.right);
                    _axes2.Add(Axis.Y, Vector2.up);
                }
                return _axes2;
            }
        }

        private static Dictionary<Axis, Vector3> _axes3;
        public static Dictionary<Axis, Vector3> Axes3
        {
            get
            {
                if (_axes3 == null)
                {
                    _axes3 = new Dictionary<Axis, Vector3>();
                    _axes3.Add(Axis.X, Vector3.right);
                    _axes3.Add(Axis.Y, Vector3.up);
                    _axes3.Add(Axis.Z, Vector3.forward);
                }
                return _axes3;
            }
        }
        
        public static Vector2 WithX(this Vector2 vector, float newX) => new Vector2(newX, vector.y);

        public static Vector2 WithY(this Vector2 vector, float newY) => new Vector2(vector.x, newY);
        
        public static Vector3 WithZ(this Vector2 vector, float newZ) => new Vector3(vector.x, vector.y, newZ);
        public static Vector3 WithX(this Vector3 vector, float newX) => new Vector3(newX, vector.y, vector.z);
 
        public static Vector3 WithY(this Vector3 vector, float newY) => new Vector3(vector.x, newY, vector.z);

        public static Vector3 WithZ(this Vector3 vector, float newZ) => new Vector3(vector.x, vector.y, newZ);
        
        public static Vector2 DegreesToVector2(this float z) =>
            new Vector2(Mathf.Cos(z * Mathf.Deg2Rad), Mathf.Sin(z * Mathf.Deg2Rad));

        public static float ToDegrees(this Vector2 vector)
        {
            vector.Normalize();
            return Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        }

        public static void Rotate(ref Vector2 vector, float degrees)
        {
            float oldDegrees = ToDegrees(vector);
            float oldDistance = vector.magnitude;
            vector = DegreesToVector2(oldDegrees + degrees) * oldDistance;
        }

        public static Vector2 Rotated(this Vector2 vector, float degrees)
        {
            Rotate(ref vector, degrees);
            return vector;
        }

        public static Vector3 RotatedY(this Vector3 vector, float degrees)
        {
            return vector.XZtoXY().Rotated(degrees).XYtoXZ().WithY(vector.y);
        }

        public static Vector2 SnapToGrid(this Vector2 point, float gridSize) =>
            point.SnapToGrid(new Vector2(gridSize, gridSize));

        public static Vector2 SnapToGrid(this Vector2 point, Vector2 gridSize)
        {
            float x = point.x;
            float y = point.y;
            x = x.Snap(gridSize.x);
            y = y.Snap(gridSize.y);
            return new Vector2(x, y);
        }

        public static Vector3 SnapToGrid(this Vector3 point, float gridSize) =>
            point.SnapToGrid(new Vector3(gridSize, gridSize, gridSize));
        
        public static Vector3 SnapToGrid(this Vector3 point, Vector3 gridSize)
        {
            float x = point.x;
            float y = point.y;
            float z = point.z;
            x = x.Snap(gridSize.x);
            y = y.Snap(gridSize.y);
            z = z.Snap(gridSize.z);
            return new Vector3(x, y, z);
        }

        public static Vector2 Clamp(this Vector2 vector, Vector2 minValues, Vector2 maxValues)
        {
            vector.x = Mathf.Clamp(vector.x, minValues.x, maxValues.x);
            vector.y = Mathf.Clamp(vector.y, minValues.y, maxValues.y);
            return vector;
        }

        public static void Clamp(ref Vector2 vector, Vector2 minValues, Vector2 maxValues)
        {
            vector = Clamp(vector, minValues, maxValues);
        }

        public static Vector2 XZtoXY(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        public static Vector3 XYtoXZ(this Vector2 vector)
        {
            return new Vector3(vector.x, 0, vector.y);
        }

        public static Vector2 RandomPointInBounds2D(this Bounds bounds)
        {
            Vector2 max = bounds.max;
            Vector2 min = bounds.min;
            return new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
        }

        public static Vector3 RandomPointInBounds(this Bounds bounds)
        {
            Vector3 max = bounds.max;
            Vector3 min = bounds.min;
            return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
        }

        public static Vector3 Average(this IEnumerable<Vector3> vectors)
        {
            Vector3[] array = vectors.ToArray();
            if (array.Length == 0)
                return Vector3.zero;
            
            float[] xs = new float[array.Length];
            float[] ys = new float[array.Length];
            float[] zs = new float[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                xs[i] = array[i].x;
                ys[i] = array[i].y;
                zs[i] = array[i].z;
            }
            return new Vector3(MathExtentions.Average(xs), MathExtentions.Average(ys), MathExtentions.Average(zs));
        }

        public static Vector2 Average(this IEnumerable<Vector2> vectors) => vectors.Select(vector => (Vector3) vector).Average();

        public static Vector2 Align(this Vector2 vector, float step)
        {
            float angle = vector.ToDegrees();
            float magnitude = vector.magnitude;
            angle = angle.Snap(step);
            return angle.DegreesToVector2() * magnitude;
        }

        public static void Align(ref Vector2 vector, float step) => vector = Align(vector, step);

        public static Vector2 IntToFloat(this Vector2Int intVector) => new Vector2(intVector.x, intVector.y);
        
        public static Vector3 IntToFloat(this Vector3Int intVector) => new Vector3(intVector.x, intVector.y, intVector.z);
        
        public static Vector2Int FloatToInt(this Vector2 floatVector) => new Vector2Int(
            Mathf.RoundToInt(floatVector.x),
            Mathf.RoundToInt(floatVector.y));
        
        public static Vector3Int FloatToInt(this Vector3 floatVector) => new Vector3Int(
            Mathf.RoundToInt(floatVector.x),
            Mathf.RoundToInt(floatVector.y),
            Mathf.RoundToInt(floatVector.z));

        public static Vector2 SetMagnitude(this Vector2 vector, float magnitude) => vector.normalized * magnitude;
        public static Vector3 SetMagnitude(this Vector3 vector, float magnitude) => vector.normalized * magnitude;

        public static bool PointRaycast(this Plane plane, Ray ray, out Vector3 point)
        {
            if (plane.Raycast(ray, out float distance))
            {
                point = ray.GetPoint(distance);
                return true;
            }
            point = Vector3.zero;
            return false;
        }

        public static Quaternion ToLookRotation2D(this Vector2 direction)
        {
            return Quaternion.Euler(0, 0, direction.ToDegrees());
        }
        
        public static Vector3 DirectionTo(this Vector3 vector, Vector3 target) => (target - vector).normalized;
        public static Vector3 DirectionTo(this Vector3 vector, Transform target) => vector.DirectionTo(target.position);

        public static Vector2 Abs(this Vector2 vector)
        {
            float x = Mathf.Abs(vector.x);
            float y = Mathf.Abs(vector.y);
            return new Vector2(x, y);
        }
        
        public static Vector3 Abs(this Vector3 vector)
        {
            float x = Mathf.Abs(vector.x);
            float y = Mathf.Abs(vector.y);
            float z = Mathf.Abs(vector.z);
            return new Vector2(x, y);
        }
        
        public static Vector2Int Abs(this Vector2Int vector)
        {
            int x = Mathf.Abs(vector.x);
            int y = Mathf.Abs(vector.y);
            return new Vector2Int(x, y);
        }

        public static float OrtogonalDistance(this Vector2 vector, Vector2 target)
        {
            Vector2 difference = (target - vector).Abs();
            return difference.x + difference.y;
        }

        public static float OrtogonalDistance(this Vector3 vector, Vector3 target)
        {
            Vector3 difference = (target - vector).Abs();
            return difference.x + difference.y + difference.z;
        }
    }

    public enum Axis
    {
        X,
        Y,
        Z
    }

}