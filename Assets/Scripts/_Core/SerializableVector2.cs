using System;
using UnityEngine;

namespace _Core
{
    [Serializable]
    public struct SerializableVector2
    {
        public float x;
        public float y;
        
        public static SerializableVector2 FromVector2(Vector2 vector2) =>  new() { x = vector2.x, y = vector2.y };
        
        public Vector2 ToVector2() => new Vector2(x, y);
    }
}