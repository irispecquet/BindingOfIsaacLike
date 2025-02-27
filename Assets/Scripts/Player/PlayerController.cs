using DefaultNamespace;
using TMPro;
using UnityEngine;
using View;

public class PlayerController : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private InputsHandler _inputs;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator2D _animator2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TMP_Text _debugStateText;

    [Header("Movement")] 
    [SerializeField] private float _speed;
    [SerializeField] private float _acceleration;

    public Vector2 CurrentMoveInputs { get; private set; }
    
    public PlayerIdleState IdleState => _idleState;
    public PlayerWalkState WalkState => _walkState;
    public Animator2D Animator => _animator2D;

    private PlayerIdleState _idleState;
    private PlayerWalkState _walkState;
    private PlayerBaseState _currentState;

    private float _lastHorizontalInput;

    private void Awake()
    {
        _idleState = new PlayerIdleState(this);
        _walkState = new PlayerWalkState(this);
    }

    private void Start()
    {
        SwitchState(IdleState);
    }

    private void Update()
    {
        CurrentMoveInputs = _inputs.CurrentMoveInputs;

        if(CurrentMoveInputs.x != 0)
            _lastHorizontalInput = CurrentMoveInputs.x;
        
        _spriteRenderer.flipX = _lastHorizontalInput < 0;
        
        _currentState.UpdateState();
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

    public Vector2 GetVelocity()
    {
        return _rigidbody.velocity;
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