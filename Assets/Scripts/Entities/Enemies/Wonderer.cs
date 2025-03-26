using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities.Enemies
{
    public class Wonderer : Enemy
    {
        [Header("References")]
        [SerializeField] private LayerMask _wallLayer;
        
        [Header("Values")]
        [SerializeField] private float _minDirectionChangeCooldown;
        [SerializeField] private float _maxDirectionChangeCooldown;
        [SerializeField] private float _rayRadius;
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationSpeed = 0.4f;
        
        private float _changeDirectionCooldown;
        private Tween _rotateTween;

        protected override void Start()
        {
            base.Start();
            _changeDirectionCooldown = 0;
        }
        
        protected override void Update()
        {
            base.Update();
            
            WanderAround();
            Move();
        }

        private void Move()
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, _rayRadius, transform.right, 0, _wallLayer);
            if (hit.collider != null)
            {
                Vector2 point = hit.point;
                Vector2 dir = new Vector3(point.x,point.y,transform.position.z) - transform.position;
                Quaternion targetRotation = Quaternion.FromToRotation(transform.right, -dir) * transform.rotation;
                
                transform.rotation = targetRotation;
                RestartDirectionCooldown();
            }
            
            Vector3 targetPosition = transform.right * (_speed * Time.deltaTime);
            _spriteRenderer.flipX = transform.right.x < 0;
            _selfTransform.Translate(targetPosition);
        }

        private void WanderAround()
        {
            _changeDirectionCooldown -= Time.deltaTime;

            if (_changeDirectionCooldown <= 0) 
                ChangeDirection();
        }

        private void ChangeDirection()
        {
            float newAngle = Random.Range(-90f, 90f);

            _rotateTween?.Kill();
            _rotateTween = transform.DORotate(new Vector3(0, 0, newAngle), _rotationSpeed);

            RestartDirectionCooldown();
        }

        private void RestartDirectionCooldown()
        {
            _changeDirectionCooldown = Random.Range(_minDirectionChangeCooldown, _maxDirectionChangeCooldown);
        }

        private void OnDestroy()
        {
            _rotateTween?.Kill();
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _rayRadius);
        }
    }
}