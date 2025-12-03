using UnityEngine;

namespace Extentions.Pause
{
    public interface IPauseSet
    {
        void PauseFromSource(MonoBehaviour source);
        void UnpauseFromSource(MonoBehaviour source);
    }
}