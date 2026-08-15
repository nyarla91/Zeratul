using UnityEngine;
using Zenject;

namespace Gameplay.Units.View
{
    public class UnitShieldView : MonoBehaviour
    {
        private static readonly int Idle = Animator.StringToHash("idle");
        private static readonly int Restore = Animator.StringToHash("restore");
        private static readonly int Full = Animator.StringToHash("full");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int Break = Animator.StringToHash("break");
        
        [SerializeField] private Animator _animator;
        [SerializeField] private Unit _unit;
        [SerializeField] private int _breakFrameWindow;

        [Inject] private GameTime GameTime { get; set; }

        private void Start()
        {
            if (_unit.HasLife)
                _unit.Life.ShieldPointsLost += TrySetHitTrigger;
        }

        private void TrySetHitTrigger(int damage)
        {
            if (_unit.Life.ShieldPoints > 0)
                _animator.SetTrigger(Hit);
        }

        private void Update()
        {
            _animator.SetTrigger(GetCurrentTrigger());
        }

        private int GetCurrentTrigger()
        {
            if ( ! _unit.HasLife || ! _unit.Life.HasShieldPoints)
                return Idle;
            if (_unit.Life.ShieldPoints == _unit.Life.MaxShieldPoints)
                return Full;
            if (_unit.Life.AreShieldsRestoring)
                return Restore;
            if (_unit.Life.ShieldPoints.Equals(0))
                return Break;
            return -1;
        }
    }
}