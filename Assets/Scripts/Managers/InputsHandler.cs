using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputsHandler : MonoBehaviour
    {
        public Action ConfirmEvent;
        
        public Vector2 CurrentMoveInputs { get; private set; }
        
        private PlayerInputs _playerInputs;
        private bool _isShooting;
        private Vector2 _lastDirection;

        private void Awake()
        {
            _playerInputs = new PlayerInputs();
        }

        private void OnEnable()
        {
            _playerInputs.Enable();
            
            _playerInputs.Movement.Move.performed += OnMove;
            _playerInputs.Movement.Move.canceled += OnMove;

            _playerInputs.Attack.Shoot.performed += OnShoot;
            _playerInputs.Attack.Shoot.canceled += OnShoot;
            
            _playerInputs.Interaction.Confirm.started += OnConfirm;
        }

        private void OnConfirm(InputAction.CallbackContext obj)
        {
            ConfirmEvent?.Invoke(); 
        }

        private void OnMove(InputAction.CallbackContext obj)
        {
            CurrentMoveInputs = obj.ReadValue<Vector2>();
        }
    
        private void OnShoot(InputAction.CallbackContext obj)
        {
            _isShooting = !obj.canceled;
            _lastDirection = obj.ReadValue<Vector2>();
        }

        public bool IsShooting(out Vector2 direction)
        {
            direction = _lastDirection;
            return _isShooting;
        }

        public void Disable()
        {
            _playerInputs.Disable();
        }

        private void OnDisable()
        {
            _playerInputs.Disable();
            
            _playerInputs.Movement.Move.performed -= OnMove;
            _playerInputs.Movement.Move.canceled -= OnMove;
        
            _playerInputs.Attack.Shoot.performed -= OnShoot;
            _playerInputs.Attack.Shoot.canceled -= OnShoot;
        }
    }
}