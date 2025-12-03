using System;
using System.Collections;
using Extentions.Pause;
using UnityEngine;

namespace Extentions
{
    public static class CoroutineExtentions
    {
        public static void Stop(this Coroutine coroutine, MonoBehaviour container, ref Coroutine fieldToClear)
        {
            coroutine.Stop(container);
            fieldToClear = null;
        }

        public static IEnumerator WaitForFixedFrames(int frames, IPauseRead pause = null, Action<int, int> frameAction = null)
        {
            for (int frame = 0; frame < frames; frame++)
            {
                frameAction?.Invoke(frame, frames);
                if (pause != null && pause.IsPaused)
                    yield return new WaitUntil(() => pause.IsUnpaused);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}