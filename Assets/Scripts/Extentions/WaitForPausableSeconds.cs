using System.Collections;
using Source.Extentions.Pause;
using UnityEngine;

namespace Source.Extentions
{
    public class PausableWaitForSeconds : CustomYieldInstruction
    {
        public override bool keepWaiting => ! _expired;

        private IPauseRead _pause;
        private bool _expired;
        
        public PausableWaitForSeconds(MonoBehaviour container, IPauseRead pause, float seconds)
        {
            _pause = pause;
            container.StartCoroutine(Await(seconds));
        }

        private IEnumerator Await(float seconds)
        {
            for (float i = 0; i < seconds; i += Time.fixedDeltaTime)
            {
                yield return new WaitUntil(() => _pause.IsUnpaused);
                yield return new WaitForFixedUpdate();
            }
            _expired = true;
        }
    }
}