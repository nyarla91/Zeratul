using _Core;
using Gameplay.Entities;
using Gameplay.Units;
using UnityEngine;
using Zenject;

namespace Gameplay.Data.Effects
{
    [CreateAssetMenu(menuName = "Gameplay Data/Effects/Spawn Sound Effect", order = 0)]
    public class SpawnSoundEffect : EffectTargetingPoint
    {
        [SerializeField] private SOInjectPresenter _gameplayPresenter;
        [SerializeField] private AudioClip[] _audioClips;
        [SerializeField] private float _pitchAmplitude;
        [SerializeField] private float _volumeScale;
        
        [Inject] private PoolFactory<SoundInstance> Factory { get; set; }
        
        public override void Apply(Unit caster, Vector2 target)
        {
            _gameplayPresenter.Inject(this);
            SoundInstance instance = Factory.Get();
            instance.transform.position = target;
            AudioClip clip = _audioClips.Random();
            instance.AudioSource.PlayPitchedOneShot(clip, _pitchAmplitude, _volumeScale);
        }
    }
}