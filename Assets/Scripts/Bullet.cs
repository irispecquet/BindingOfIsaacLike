using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private bool _canMove;
    private Vector2 _direction;
    
    public void Init(Vector2 direction)
    {
        _direction = direction;
        _canMove = true;
    }

    private void Update()
    {
        if (!_canMove)
            return;
        
        transform.Translate(_direction * (_speed * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
