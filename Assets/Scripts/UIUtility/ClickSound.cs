using _Core;
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
            AudioSource.PlayPitchedOneShot(_clip, _pitchAmplitude);
        }

        public void PlayToggle(bool toggle)
        {
            AudioSource.PlayPitchedOneShot(toggle ? _clip : _negativeClip, _pitchAmplitude);
        }
    }
}