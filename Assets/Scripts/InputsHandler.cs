using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class InputsHandler : MonoBehaviour
    {
        public Vector2 CurrentMoveInputs { get; private set; }
        
        private PlayerInputs _playerInputs;

        private void Awake()
        {
            _playerInputs = new PlayerInputs();
        }

        private void OnEnable()
        {
            _playerInputs.Enable();
            
            _playerInputs.Movement.Move.performed += OnMove;
            _playerInputs.Movement.Move.canceled += OnMove;
        }

        private void OnMove(InputAction.CallbackContext obj)
        {
            CurrentMoveInputs = obj.ReadValue<Vector2>();
        }

        private void OnDisable()
        {
            _playerInputs.Disable();
            
            _playerInputs.Movement.Move.performed -= OnMove;
            _playerInputs.Movement.Move.canceled -= OnMove;
        }
    }
}