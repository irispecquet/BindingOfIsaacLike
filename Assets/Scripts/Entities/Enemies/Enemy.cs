using System;
using UnityEngine;

namespace Entities.Enemies
{
    public class Enemy : Entity
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected Transform _selfTransform;


        protected override void Die()
        {
            base.Die();
            Destroy(_selfTransform.gameObject);
        }
    }
}