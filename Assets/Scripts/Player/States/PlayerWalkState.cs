using System;
using Player;
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
            float magnitude = _playerController.GetVelocity().magnitude;
            
            if (magnitude < 0.02f)
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