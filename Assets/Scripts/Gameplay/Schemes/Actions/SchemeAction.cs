using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Schemes.Actions
{
    public abstract class SchemeAction : MonoBehaviour
    {
        public abstract UniTask Act();
    }
}