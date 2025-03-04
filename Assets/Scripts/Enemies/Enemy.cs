using UnityEngine;
using View;

namespace Enemies
{
    public abstract class Enemy : MonoBehaviour, IHittable
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected Transform _selfTransform;
        [SerializeField] protected int _initLife;
        
        protected int _currentLife;

        protected virtual void Start()
        {
            _currentLife = _initLife;
        }

        private void TakeDamage(int damage)
        {
            _currentLife -= damage;

            if (_currentLife <= 0)
                Die();
        }

        private void Die()
        {
            Destroy(_selfTransform.gameObject);
        }

        public void Hit(int damage)
        {
            TakeDamage(damage);
        }
    }
}