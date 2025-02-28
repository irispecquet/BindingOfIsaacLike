using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
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
        
        public Vector2 CurrentMoveInputs { get; private set; }
    
        public PlayerIdleState IdleState => _idleState;
        public PlayerWalkState WalkState => _walkState;

        private PlayerIdleState _idleState;
        private PlayerWalkState _walkState;
        private PlayerBaseState _currentState;

        private float _lastHorizontalInput;
        private float _cooldownTimer;
        private bool _isShooting;

        private void Awake()
        {
            _idleState = new PlayerIdleState(this);
            _walkState = new PlayerWalkState(this);
        }

        private void Start()
        {
            SwitchState(IdleState);
            _cooldownTimer = 0;
        }

        private void Update()
        {
            CurrentMoveInputs = _inputs.CurrentMoveInputs;
            _currentState.UpdateState();

            if (_cooldownTimer > 0)
                _cooldownTimer -= Time.deltaTime;

            if (_inputs.IsShooting(out Vector2 direction))
            {
                if (_cooldownTimer <= 0)
                {
                    Instantiate(_bulletPrefab, transform.position, Quaternion.identity).Init(direction);
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
                }
            }
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdateState();
        }

        public void Move()
        {
            Vector2 targetVelocity = CurrentMoveInputs.normalized * _speed;
            SetVelocity(targetVelocity, _acceleration * Time.fixedDeltaTime);
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
    }
}