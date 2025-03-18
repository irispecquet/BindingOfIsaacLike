using Interfaces;
using UnityEngine;

public class Bullet : MonoBehaviour, IHitter
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    
    private bool _canMove;
    private GameObject _owner;
    private Vector2 _direction;
    
    public void Init(Vector2 direction, GameObject owner)
    {
        if (direction.x != 0)
            direction.y = 0;
        
        _direction = direction;
        _canMove = true;
        _owner = owner;
    }

    private void Update()
    {
        if (!_canMove)
            return;
        
        transform.Translate(_direction * (_speed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject == _owner)
            return;
        
        if(other.gameObject.TryGetComponent(out IHittable hittable))
            OnHit(hittable);
        
        Destroy(gameObject);
    }

    public void OnHit(IHittable hittable)
    {
        hittable.Hit(_damage);
    }
}
