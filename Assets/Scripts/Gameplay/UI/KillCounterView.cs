using Gameplay.Player;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gameplay.UI
{
    public class KillCounterView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _counter;
        
        [Inject] private PlayerControlResources PlayerControlResources { get; set; }

        private void Update()
        {
            _canvasGroup.alpha = PlayerControlResources.KillCounter > 0 ? 1 : 0;
            _counter.text = PlayerControlResources.KillCounter.ToString();
        }
    }
}