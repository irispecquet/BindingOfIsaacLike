using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerWalkState : PlayerBaseState
    {
        public PlayerWalkState(PlayerController playerController) : base(playerController)
        {
        }

        public override void EnterState()
        {
        }

        public override void UpdateState()
        {
            Vector2 currentInputs = _playerController.CurrentMoveInputs;
            
            if(currentInputs == Vector2.zero)
                _playerController.SwitchState(_playerController.IdleState);
        }

        public override void FixedUpdateState()
        {
            _playerController.Move();
        }

        public override void ExitState()
        {
            _playerController.SetVelocity(Vector2.zero, 0);
        }
    }
}