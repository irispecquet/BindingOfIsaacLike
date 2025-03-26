using System;
using System.Collections;
using UnityEngine;

namespace Entities.Enemies
{
    public class Enemy : Entity
    {
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        [SerializeField] protected Transform _selfTransform;
        [SerializeField] private float _damageColorTimer = 0.1f;

        public override void Hit(int damage)
        {
            base.Hit(damage);
            
            StopAllCoroutines();
            _spriteRenderer.color = Color.red;
            StartCoroutine(WaitToChangeColor(_damageColorTimer));
        }

        private IEnumerator WaitToChangeColor(float timer)
        {
            yield return new WaitForSeconds(timer);
            _spriteRenderer.color = Color.white;
        }

        protected override void Die()
        {
            base.Die();
            Destroy(_selfTransform.gameObject);
        }
    }
}