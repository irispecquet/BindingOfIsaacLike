using UnityEngine;

namespace Entities.Enemies
{
    public class Pooter : Wonderer
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private float _playerDetectionRayLenght;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private float _shootCooldown;
        
        private float _currentShootCooldown;
        private int _currentHorizontalDirection;
        private bool _canShoot;

        protected override void Update()
        {
            base.Update();
            
            _currentHorizontalDirection = transform.right.x >= 0 ? 1 : -1;

            RefreshShootingCooldown();
            
            if(PlayerIsInSight() && _canShoot)
                ShootBullet();
        }

        private bool PlayerIsInSight()
        {
            return Physics2D.Raycast(transform.position, new Vector2(_currentHorizontalDirection, 0), _playerDetectionRayLenght, _playerLayer);
        }

        private void RefreshShootingCooldown()
        {
            _currentShootCooldown -= Time.deltaTime;

            if (_currentShootCooldown <= 0)
                _canShoot = true;
        }

        private void ShootBullet()
        {
            Instantiate(_bulletPrefab, transform.position, Quaternion.identity).Init(new Vector2(_currentHorizontalDirection, 0), gameObject);
            _currentShootCooldown = _shootCooldown;
            _canShoot = false;
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + new Vector3(_currentHorizontalDirection, 0) * _playerDetectionRayLenght);
        }
    }
}