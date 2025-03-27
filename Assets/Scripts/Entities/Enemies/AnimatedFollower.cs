using System;
using LuniLib.View;
using UnityEngine;

namespace Entities.Enemies
{
    public class AnimatedFollower : Follower
    {
        [SerializeField] private Animator2D _bodyAnimator;

        protected override void Update()
        {
            base.Update();
            
            UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            Vector2 dir = _selfTransform.position - _player.transform.position;
            dir.Normalize();

            _bodySpriteRenderer.flipX = dir.x > 0;

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