using UnityEngine;
using Zenject;

namespace UIUtility
{
    public class ClickSound : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private AudioClip _negativeClip;
        [SerializeField] private float _pitchAmplitude;

        [Inject] private AudioSource AudioSource { get; set; }

        public void Play()
        {
            PlayClip(_clip);
        }

        public void PlayToggle(bool toggle)
        {
            PlayClip(toggle ? _clip : _negativeClip);
        }

        private void PlayClip(AudioClip clip)
        {
            AudioSource.pitch = 1 - Random.value * _pitchAmplitude + Random.value * _pitchAmplitude;
            AudioSource.PlayOneShot(clip);
        }
    }
}