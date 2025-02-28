using System;
using UnityEngine;
using UnityEngine.Serialization;
using View;

namespace DefaultNamespace.Player
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Animator2D _bodyAnimator;
        [SerializeField] private SpriteRenderer _headSpriteRenderer;
        [SerializeField] private SpriteRenderer _bodySpriteRenderer;
        [SerializeField] private PlayerController _playerController;

        private Vector2 _playerInputs;
        private float _lastHorizontalInput;
        
        private void Update()
        {
            _playerInputs = _playerController.CurrentMoveInputs;

            if (_playerInputs.x != 0)
                _lastHorizontalInput = _playerInputs.x;

            HandleAnimations();
        }

        private void HandleAnimations()
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
            {
                _bodyAnimator.PlayStateAnimation("WalkDown");
            }
        }
    }
}