using Entities;
using UnityEngine;

public class Destroyable : Entity
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Sprite[] _sprites;
        
    private int _spriteIndex;

    protected override void Start()
    {
        base.Start();
        _spriteIndex = 0;
        _spriteRenderer.sprite = _sprites[_spriteIndex];
    }

    public override void Hit(int damage)
    {
        base.Hit(damage);
        
        if(_spriteIndex < _sprites.Length)
            _spriteRenderer.sprite = _sprites[_spriteIndex++];
    }

    protected override void Die()
    {
        base.Die();
        _collider.enabled = false;
        CurrentRoomNode.IsOccupied = false;
        this.enabled = false;
    }
}