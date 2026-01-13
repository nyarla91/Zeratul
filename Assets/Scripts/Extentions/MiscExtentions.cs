using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Extentions
{
    public static class MiscExtentions
    {
        public static Vector3 DirectionTo(this Transform transform, Vector3 target) => (target - transform.position).normalized;
        public static Vector3 DirectionTo(this Transform transform, Transform target) => transform.DirectionTo(target.position);
        public static Vector2 DirectionTo2D(this Transform transform, Vector2 target) => (target - (Vector2) transform.position).normalized;
        public static Vector2 DirectionTo2D(this Transform transform, Transform target) => transform.DirectionTo2D(target.position);

        public static async Task WaitForCondition<T>(this T original, Func<T, bool> condition, int checkPeriod)
        {
            while (!condition.Invoke(original))
            {
                await Task.Delay(checkPeriod);
            }
        }

        public static T With<T>(this T obj, Action<T> action)
        {
            action.Invoke(obj);
            return obj;
        }

        public static bool Includes(this LayerMask mask, int layer) => mask == (mask | (1 << layer));

        public static Vector3 AverageNormal(this Collision collision) =>
            collision.contacts.Length > 0 ? collision.contacts.Select(point => point.normal).Average() : Vector3.zero;

        public static void Fill(this RectMask2D rectMask2D, bool vertical, bool toBottomLeft, float fillPercent)
        {
            RectTransform rectTransform = rectMask2D.rectTransform;
            float fullPadding = vertical ? rectTransform.rect.height : rectTransform.rect.width;
            float padding = Mathf.Lerp(fullPadding, 0, fillPercent);
            rectMask2D.padding = vertical
                ? (toBottomLeft ? new Vector4(0, padding, 0, 0) : new Vector4(0, 0, 0, padding))
                : (toBottomLeft ? new Vector4(padding, 0, 0, 0) : new Vector4(0, 0, padding, 0));
        }

        public static Gradient ToGradient(this Color color)
        {
            Gradient result = new();
            GradientColorKey colorKeys = new GradientColorKey(color, 0);
            GradientAlphaKey alphaKeys = new GradientAlphaKey(color.a, 0);
            result.SetKeys( new[]{colorKeys}, new[]{alphaKeys});
            return result;
        }
    }
}