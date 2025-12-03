using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extentions.Pause
{
    public class Pause : IPauseRead, IPauseSet
    {
        private List<MonoBehaviour> _pauseSources = new List<MonoBehaviour>();

        public bool IsPaused
        {
            get
            {
                ValidatePauseSources();
                return _pauseSources.Count > 0;
            }
        }

        public bool IsUnpaused => ! IsPaused;
        
        public void PauseFromSource(MonoBehaviour source) => _pauseSources.Add(source);
        public void UnpauseFromSource(MonoBehaviour source) => _pauseSources.TryRemove(source);

        private void ValidatePauseSources()
        {
            _pauseSources = _pauseSources.Where(source => source != null).ToList();
        }
    }
}