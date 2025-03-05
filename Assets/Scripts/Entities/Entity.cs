using Interfaces;
using UnityEngine;

namespace Entities
{
    public abstract class Entity : MonoBehaviour, IHittable
    {
        [SerializeField] protected int _initLife;

        protected int _currentLife;

        protected virtual void Start()
        {
            _currentLife = _initLife;
        }

        protected virtual void TakeDamage(int damage)
        {
            SetLife(_currentLife - damage);

            if (_currentLife <= 0)
                Die();
        }

        private void SetLife(int value)
        {
            _currentLife = value;
        }

        protected virtual void Die()
        {
        }

        public void Hit(int damage)
        {
            TakeDamage(damage);
        }
    }
}