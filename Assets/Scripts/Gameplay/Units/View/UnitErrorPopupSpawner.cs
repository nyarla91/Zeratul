using _Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Units.View
{
    public class UnitErrorPopupSpawner : MonoBehaviour
    {
        [SerializeField] private Unit _unit;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _clip;
        [SerializeField] private float _volumeScale;
        [SerializeField] private float _pitchAmplitude;

        [Inject] private PoolFactory<ErrorPopup> ErrorPopupFactory { get; set; }

        private void Start()
        {
            _unit.Orders.OrderCancelled += SpawnErrorPopup;
        }

        private void SpawnErrorPopup(string errorMessage)
        {
            if ( ! _unit.Alliance.OwnedByPlayer)
                return;
            ErrorPopup errorPopup = ErrorPopupFactory.Get();
            errorPopup.InitError(transform.position, errorMessage);
            _audioSource.PlayPitchedOneShot(_clip, _pitchAmplitude, _volumeScale);
        }
    }
}
