using System;
using Interfaces;
using UnityEngine;

namespace Entities
{
    public abstract class Entity : MonoBehaviour, IHittable
    {
        [SerializeField] protected int _initLife;

        public Action<Entity> DieEvent;
        public Action<Entity> HurtEvent;

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
            else
                HurtEvent?.Invoke(this);
        }

        private void SetLife(int value)
        {
            _currentLife = value;
        }

        protected virtual void Die()
        {
            DieEvent?.Invoke(this);
        }

        public void Hit(int damage)
        {
            TakeDamage(damage);
        }
    }
}