using UnityEngine;

namespace Extentions.Pause
{
    public interface IPauseSet
    {
        void Pause(MonoBehaviour source);
        void Unpause(MonoBehaviour source);
    }
}