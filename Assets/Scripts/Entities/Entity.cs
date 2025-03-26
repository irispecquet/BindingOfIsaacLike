using System;
using Interfaces;
using Managers;
using Rooms;
using UnityEngine;

namespace Entities
{
    public abstract class Entity : MonoBehaviour, IHittable, IHitter
    {
        [SerializeField] protected int _initLife;
        [SerializeField] protected bool _doDamageOnTouch;
        [SerializeField] private int _damage;

        public Action<Entity> DieEvent;
        public Action<Entity> HurtEvent;
        
        public RoomNode CurrentRoomNode { get; private set; }

        protected int _currentLife;

        protected virtual void Start()
        {
            _currentLife = _initLife;
            RefreshCurrentRoomNode();
        }

        protected virtual void Update()
        {
            RefreshCurrentRoomNode();
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

        public virtual void Hit(int damage)
        {
            TakeDamage(damage);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ApplyDamage(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            ApplyDamage(other);
        }

        private void ApplyDamage(Collider2D other)
        {
            if (_doDamageOnTouch)
            {
                if (other.gameObject.TryGetComponent(out IHittable hittable))
                    OnHit(hittable);
            }
        }
        
        public void OnHit(IHittable hittable)
        {
            hittable.Hit(_damage);
        }
        
        private void RefreshCurrentRoomNode()
        {
            Room room = GameManager.Instance.RoomManager.CurrentRoom;

            if (!room.IsSet)
                return;

            foreach (RoomNode node in room.Nodes)
            {
                if (node.ContainsPosition(transform.position))
                {
                    CurrentRoomNode = node;
                    break;
                }
            }
        }
    }
}