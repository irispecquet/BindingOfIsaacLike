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
            
            if(currentInputs.x != 0 && currentInputs.y == 0)
                _playerController.Animator.PlayStateAnimation("WalkRight");
            
            if(currentInputs.y > 0)
                _playerController.Animator.PlayStateAnimation("WalkUp");
            else if(currentInputs.y < 0)
                _playerController.Animator.PlayStateAnimation("WalkDown");
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