using System;
using System.Collections;
using UnityEngine;

namespace Entities.Enemies
{
    public class Enemy : Entity
    {
        [SerializeField] protected SpriteRenderer _bodySpriteRenderer;
        [SerializeField] protected SpriteRenderer _headSpriteRenderer;
        [SerializeField] protected Transform _selfTransform;
        [SerializeField] private float _damageColorTimer = 0.1f;

        public override void Hit(int damage)
        {
            base.Hit(damage);
            
            StopAllCoroutines();
            SetColor(Color.red);
            StartCoroutine(WaitToChangeColor(_damageColorTimer));
        }

        private void SetColor(Color color)
        {
            if(_headSpriteRenderer != null)
                _headSpriteRenderer.color = color;
            
            _bodySpriteRenderer.color = color;
        }

        private IEnumerator WaitToChangeColor(float timer)
        {
            yield return new WaitForSeconds(timer);
            SetColor(Color.white);
        }

        protected override void Die()
        {
            base.Die();
            Destroy(_selfTransform.gameObject);
        }
    }
}