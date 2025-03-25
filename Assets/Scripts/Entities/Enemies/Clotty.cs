using UnityEngine;

namespace Entities.Enemies
{
    public class Clotty : Wonderer
    {
        [Header("References")] 
        [SerializeField] private Bullet _bulletPrefab;

        [Header("Values")] 
        [SerializeField] private float _shootCooldown;

        private float _currentShootCooldown;

        protected override void Start()
        {
            base.Start();
            _currentShootCooldown = 0;
        }

        protected override void Update()
        {
            base.Update();
            RefreshShootingCooldown();
        }

        private void RefreshShootingCooldown()
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
                Instantiate(_bulletPrefab, transform.position, Quaternion.identity).Init(dir, gameObject, true);
        }

        private Vector2[] GetAllCardinalDirections()
        {
            return new[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        }
    }
}