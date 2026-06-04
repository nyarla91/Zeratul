using System.Collections.Generic;
using System.Linq;

namespace _Core.Pause
{
    public class GamePause : IPauseReadonly
    {
        private List<object> _sources = new();
        
        public virtual bool IsPaused
        {
            get
            {
                ValidatePauseSources();
                return _sources.Count > 0;
            }
        }

        public virtual bool IsUnpaused => ! IsPaused;
        
        public void Pause(object source)
        {
            if (IsPausedFrom(source))
                return;
            _sources.Add(source);
        }

        public void Unpause(object source) => _sources.Remove(source);

        public bool IsPausedFrom(object source) => _sources.Contains(source);
        
        private void ValidatePauseSources()
        {
            _sources = _sources.Where(source => source != null).ToList();
        }
    }
}