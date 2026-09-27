using Gameplay.Player;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay.Audio
{
    public class GlobalEventSoundEffects : MonoBehaviour
    {
        [SerializeField] private float _pitchAmplitude;
        [SerializeField] private AudioClip _orderClip;
        
        [Inject] private PlayerOrdersDispatcher PlayerOrdersDispatcher { get; set; }
        [Inject] private AudioSource AudioSource { get; set; }

        private void Awake()
        {
            PlayerOrdersDispatcher.OrderIssued += () => PlayClip(_orderClip);
        }

        private void PlayClip(AudioClip clip)
        {
            AudioSource.pitch = 1 - Random.value * _pitchAmplitude + Random.value * _pitchAmplitude;
            AudioSource.PlayOneShot(clip);
        }
    }
}