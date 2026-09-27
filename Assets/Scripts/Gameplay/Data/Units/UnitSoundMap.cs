using UnityEngine;

namespace Gameplay.Data.Units
{
    [CreateAssetMenu(menuName = "Gameplay Data/Unit/Unit Sound Map", order = 0)]
    public class UnitSoundMap : ScriptableObject
    {
        [SerializeField] private AudioClip[] _stepClips;
        [SerializeField] private float _stepDistance;

        public AudioClip[] StepClips => _stepClips;
        public float StepDistance => _stepDistance;
    }
}