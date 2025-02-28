using Player;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerIdleState : PlayerBaseState
    {
        public PlayerIdleState(PlayerController playerController) : base(playerController)
        {
        }

        public override void EnterState()
        {
        }

        public override void UpdateState()
        {
            if(_playerController.CurrentMoveInputs != Vector2.zero)
                _playerController.SwitchState(_playerController.WalkState);
        }

        public override void FixedUpdateState()
        {
        }

        public override void ExitState()
        {
        }
    }
}