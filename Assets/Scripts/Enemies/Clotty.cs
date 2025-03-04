using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Clotty : Enemy
    {
        [Header("References")]
        [SerializeField] private LayerMask _wallLayer;
        [SerializeField] private Bullet _bulletPrefab;
        
        [Header("Values")]
        [SerializeField] private float _minDirectionChangeCooldown;
        [SerializeField] private float _maxDirectionChangeCooldown;
        [SerializeField] private float _shootCooldown;
        [SerializeField] private float _rayRadius;
        [SerializeField] private float _speed;
        
        private float _changeDirectionCooldown;
        private float _currentShootCooldown;

        protected override void Start()
        {
            base.Start();
            _changeDirectionCooldown = 0;
            _currentShootCooldown = 0;
        }

        private void Update()
        {
            WanderAround();
            Move();
            Shoot();
        }

        private void Shoot()
        {
            _currentShootCooldown -= Time.deltaTime;

            if (_currentShootCooldown <= 0)
            {
                ShootBullet();
                _currentShootCooldown = _shootCooldown;
            }
        }

        private void ShootBullet()
        {
            Vector2[] allCardinalDirections = GetAllCardinalDirections();
            
            foreach (Vector2 dir in allCardinalDirections)
                Instantiate(_bulletPrefab, transform.position, Quaternion.identity).Init(dir, gameObject);
        }

        private Vector2[] GetAllCardinalDirections()
        {
            return new []{Vector2.up, Vector2.down, Vector2.left, Vector2.right};
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
            transform.Rotate(0, 0, newAngle);
                
            RestartDirectionCooldown();
        }

        private void RestartDirectionCooldown()
        {
            _changeDirectionCooldown = Random.Range(_minDirectionChangeCooldown, _maxDirectionChangeCooldown);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _rayRadius);
        }
    }
}