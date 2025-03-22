using System;
using UnityEngine;

namespace Entities.Enemies.Shooting
{
    public class ShootOnSight : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private TargetDetectionType _detectionType;
        [SerializeField] private float _targetDetectionRayLenght;
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private float _shootCooldown;
        
        private float _currentShootCooldown;
        private int _currentHorizontalDirection;
        private bool _canShoot;
        
        private void Start()
        {
            RefreshShootingCooldown();
        }

        private void Update()
        {
            RefreshShootingCooldown();
            
            _currentHorizontalDirection = transform.right.x >= 0 ? 1 : -1;
            
            if(_canShoot && TargetIsInSight(out Vector2 direction))
                ShootBullet(direction);
        }
        
        private bool TargetIsInSight(out Vector2 shootDirection)
        {
            bool targetIsInSight = false;
            shootDirection = Vector2.zero;

            if (_detectionType == TargetDetectionType.DIRECTIONAL)
            {
                Vector2 direction = new Vector2(_currentHorizontalDirection, 0);
                targetIsInSight = Physics2D.Raycast(transform.position, direction, _targetDetectionRayLenght, _targetLayer);
                shootDirection = direction;
            }
            else if (_detectionType == TargetDetectionType.RADIUS)
            {
                RaycastHit2D raycastHit2D = Physics2D.CircleCast(transform.position, _targetDetectionRayLenght, Vector2.zero, 100f, _targetLayer);
                targetIsInSight = raycastHit2D;
                shootDirection = raycastHit2D.point - (Vector2)transform.position;
            }

            return targetIsInSight;
        }

        private void RefreshShootingCooldown()
        {
            _currentShootCooldown -= Time.deltaTime;

            if (_currentShootCooldown <= 0)
                _canShoot = true;
        }

        private void ShootBullet(Vector2 shootDirection)
        {
            Instantiate(_bulletPrefab, transform.position, Quaternion.identity).Init(shootDirection, gameObject);
            _currentShootCooldown = _shootCooldown;
            _canShoot = false;
        }
        
        private void OnDrawGizmosSelected()
        {
            if (_detectionType == TargetDetectionType.DIRECTIONAL)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, transform.position + new Vector3(_currentHorizontalDirection, 0) * _targetDetectionRayLenght);
            }
            else if (_detectionType == TargetDetectionType.RADIUS)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, _targetDetectionRayLenght);
            }
        }
    }

    public enum TargetDetectionType
    {
        DIRECTIONAL,
        RADIUS
    }
}