using System;
using LuniLib.View;
using Managers;
using UnityEngine;

namespace Entities.Enemies
{
    public class Gaper : Enemy
    {
        [SerializeField] private float _speed;
        [SerializeField] private Animator2D _bodyAnimator;

        private Transform _playerTransform;

        protected override void Start()
        {
            base.Start();

            _playerTransform = GameManager.Instance.PlayerController.transform;
        }

        private void Update()
        {
            Vector2 dir = _selfTransform.position - _playerTransform.position;
            dir.Normalize();
            
            _spriteRenderer.flipX = dir.x > 0;
            _selfTransform.position = Vector3.MoveTowards(_selfTransform.position, _playerTransform.position, _speed * Time.deltaTime);
            
            if (Math.Abs(dir.x) > 0.5f)
            {
                _bodyAnimator.PlayStateAnimation("WalkRight");
                return;
            }

            if (Math.Abs(dir.y) > 0.5f)
                _bodyAnimator.PlayStateAnimation("WalkDown");
        }
    }
}