using System;
using LuniLib.View;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Animator2D _headAnimator;
        [SerializeField] private Animator2D _bodyAnimator;
        [SerializeField] private SpriteRenderer _headSpriteRenderer;
        [SerializeField] private SpriteRenderer _bodySpriteRenderer;
        [SerializeField] private PlayerController _playerController;

        public Action DieAnimationFinishedEvent;
        
        private Vector2 _playerInputs;
        private float _lastHorizontalInput;
        private bool _isShooting;
        private bool _isDead;

        private void Start()
        {
            _playerController.ShootEvent += OnShoot;
            _playerController.CancelShootEvent += OnCancelShoot;
            
            _playerController.HurtEvent += OnPlayerHurt;
            _playerController.DieEvent += OnPlayerDied;
        }

        private void OnPlayerDied(Entity player)
        {
            _isDead = true;
            _headSpriteRenderer.enabled = false;
            _bodyAnimator.PlayActionAnimation("Die", null, () => DieAnimationFinishedEvent?.Invoke());
        }

        private void OnPlayerHurt(Entity player)
        {
            _headSpriteRenderer.enabled = false;
            _bodyAnimator.PlayActionAnimation("Hurt", null, () => _headSpriteRenderer.enabled = true);
        }

        private void Update()
        {
            _playerInputs = _playerController.CurrentMoveInputs;

            if (_playerInputs.x != 0)
                _lastHorizontalInput = _playerInputs.x;

            HandleBodyAnimations();
            
            if(!_isShooting)
                _headAnimator.PlayStateAnimation("Idle");
        }

        private void HandleBodyAnimations()
        {
            if (_isDead)
                return;
            
            _bodySpriteRenderer.flipX = _lastHorizontalInput < 0;

            if (_playerInputs == Vector2.zero)
            {
                _bodyAnimator.PlayStateAnimation("Idle");
                return;
            }

            if (_playerInputs.x != 0)
            {
                _bodyAnimator.PlayStateAnimation("WalkRight");
                return;
            }

            if (_playerInputs.y != 0)
                _bodyAnimator.PlayStateAnimation("WalkDown");
        }

        private void OnShoot(Vector2 direction)
        {
            _isShooting = true;
            
            if (direction.x != 0)
            {
                _headSpriteRenderer.flipX = direction.x < 0;
                _headAnimator.PlayActionAnimation("BlinkRight", "LookRight");
                return;
            }

            if (direction.y > 0)
                _headAnimator.PlayActionAnimation("BlinkUp", "LookUp");
            else if (direction.y < 0)
                _headAnimator.PlayActionAnimation("BlinkDown", "LookDown");
        }

        private void OnCancelShoot()
        {
            _isShooting = false;
        }
    }
}