using System;
using Entities.Player.States;
using Interfaces;
using Managers;
using Rooms;
using TMPro;
using UnityEngine;

namespace Entities.Player
{
    public class PlayerController : Entity, IHittable
    {
        [Header("References")] 
        [SerializeField] private InputsHandler _inputs;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private TMP_Text _debugStateText;
        [SerializeField] private Bullet _bulletPrefab;

        [Header("Values")] 
        [SerializeField] private float _speed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _shootCooldown;
        public event Action<Vector2> ShootEvent;
        public event Action CancelShootEvent;
        public event Action<int> RefreshLife;
        
        public Vector2 CurrentMoveInputs { get; private set; }
        public RoomNode CurrentRoomNode { get; private set; }
    
        public PlayerIdleState IdleState => _idleState;
        public PlayerWalkState WalkState => _walkState;

        private PlayerIdleState _idleState;
        private PlayerWalkState _walkState;
        private PlayerBaseState _currentState;

        private float _lastHorizontalInput;
        private float _cooldownTimer;
        private bool _isShooting;

        #region UNITY METHODS

        private void Awake()
        {
            _idleState = new PlayerIdleState(this);
            _walkState = new PlayerWalkState(this);
        }

        protected override void Start()
        {
            base.Start();

            SwitchState(IdleState);
            _cooldownTimer = 0;
            RefreshCurrentRoomNode();
        }

        private void Update()
        {
            CurrentMoveInputs = _inputs.CurrentMoveInputs;
            _currentState.UpdateState();

            if (_cooldownTimer > 0)
                _cooldownTimer -= Time.deltaTime;

            HandleShooting();
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdateState();
        }

        #endregion // UNITY METHODS

        private void HandleShooting()
        {
            if (_inputs.IsShooting(out Vector2 direction))
            {
                if (_cooldownTimer <= 0)
                {
                    Instantiate(_bulletPrefab, transform.position, Quaternion.identity).Init(direction, gameObject, true);
                    ShootEvent?.Invoke(direction);
                    _cooldownTimer = _shootCooldown;
                    _isShooting = true;
                }
            }
            else
            {
                if (_isShooting)
                {
                    _isShooting = false;
                    CancelShootEvent?.Invoke();
                    // _cooldownTimer = 0;
                }
            }
        }
        
        public void Move()
        {
            Vector2 targetVelocity = CurrentMoveInputs.normalized * _speed;
            SetVelocity(targetVelocity, _acceleration * Time.fixedDeltaTime);
            RefreshCurrentRoomNode();
        }
        
        public void SetVelocity(Vector2 targetVelocity, float acceleration)
        { 
            Vector2 newVelocity = acceleration != 0 ? Vector2.Lerp(_rigidbody.velocity, targetVelocity, acceleration) : targetVelocity;
            _rigidbody.velocity = newVelocity;
        }

        public void SwitchState(PlayerBaseState newState)
        {
            if (_currentState != null)
                _currentState.ExitState();
        
            _currentState = newState;
            _currentState.EnterState();
        
            _debugStateText.text = _currentState.ToString();
        }

        private void RefreshCurrentRoomNode()
        {
            Room room = GameManager.Instance.RoomManager.CurrentRoom;

            foreach (RoomNode node in room.Nodes)
            {
                if (node.ContainsPosition(transform.position))
                {
                    CurrentRoomNode = node;
                    break;
                }
            }
        }

        public Vector2 GetVelocity()
        {
            return _rigidbody.velocity;
        }

        protected override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            RefreshLife?.Invoke(_currentLife);
        }

        protected override void Die()
        {
            base.Die();
            _inputs.Disable();
        }
    }
}