using System;
using UnityEngine;
using View;

namespace Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Animator2D _headAnimator;
        [SerializeField] private Animator2D _bodyAnimator;
        [SerializeField] private SpriteRenderer _headSpriteRenderer;
        [SerializeField] private SpriteRenderer _bodySpriteRenderer;
        [SerializeField] private PlayerController _playerController;

        private Vector2 _playerInputs;
        private float _lastHorizontalInput;

        private void Start()
        {
            _playerController.ShootEvent += OnShoot;
            _playerController.CancelShootEvent += OnCancelShoot;
        }

        private void Update()
        {
            _playerInputs = _playerController.CurrentMoveInputs;

            if (_playerInputs.x != 0)
                _lastHorizontalInput = _playerInputs.x;

            HandleBodyAnimations();
        }

        private void HandleBodyAnimations()
        {
            _bodySpriteRenderer.flipX = _lastHorizontalInput < 0;

            if (_playerInputs == Vector2.zero)
            {
                _bodyAnimator.PlayStateAnimation("Idle");
                return;
            }
            
            if(_playerInputs.x != 0)
            {
                _bodyAnimator.PlayStateAnimation("WalkRight");
                return;
            }

            if (_playerInputs.y != 0)
                _bodyAnimator.PlayStateAnimation("WalkDown");
        }

        private void OnShoot(Vector2 direction)
        {
            if (direction.x != 0)
            {
                _headSpriteRenderer.flipX = direction.x < 0;
                _headAnimator.PlayStateAnimation("LookRight");
                
                return;
            }

            if (direction.y > 0)
                _headAnimator.PlayStateAnimation("LookUp");
            else if (direction.y < 0)
                _headAnimator.PlayStateAnimation("LookDown");
        }
        
        private void OnCancelShoot()
        {
            _headAnimator.PlayStateAnimation("Idle");
        }
    }
}