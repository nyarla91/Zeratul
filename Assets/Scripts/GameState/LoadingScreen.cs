using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GameState
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [SerializeField] private Image _animation;
        [SerializeField] private float _transitionDuration;
        [SerializeField] private Sprite[] _transitionFrames;

        private float _transitionT;
        private float _targetTransitionT;
        
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public async UniTask Show()
        {
            gameObject.SetActive(true);
            _animation.color = Color.clear;
            _targetTransitionT = 1;
            await UniTask.WaitUntil(() => _transitionT.Equals(1));
            _animation.color = Color.white;
        }
        
        public async UniTask Hide()
        {
            _animation.color = Color.clear;
            _targetTransitionT = 0;
            await UniTask.WaitUntil(() => _transitionT.Equals(1));
            gameObject.SetActive(true);
        }

        private void Update()
        {
            float delta = 1 / _transitionDuration * Time.deltaTime;
            _transitionT = Mathf.MoveTowards(_transitionT, _targetTransitionT, delta);
            
            int maxFrame = _transitionFrames.Length - 1;
            int frame = Mathf.RoundToInt(_transitionT * maxFrame);
            
            _background.sprite = _transitionFrames[frame];
        }
    }
}