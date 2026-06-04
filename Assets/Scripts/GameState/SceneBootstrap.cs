using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameState
{
    public abstract class SceneBootstrap : MonoBehaviour
    {
        public abstract UniTask Initialize();
    }
}