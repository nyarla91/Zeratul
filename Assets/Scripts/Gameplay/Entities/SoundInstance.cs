using System;
using UnityEngine;

namespace Gameplay.Entities
{
    public class SoundInstance : PoolElement<SoundInstance>
    {
        [SerializeField] private AudioSource _audioSource;

        public AudioSource AudioSource => _audioSource;

        public override void OnSpawn() { }

        protected override void OnDespawn() { }

        private void FixedUpdate()
        {
            if (_audioSource.clip != null && ! _audioSource.isPlaying)
                Despawn();
        }
    }
}