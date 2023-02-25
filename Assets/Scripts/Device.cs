
using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public abstract class Device : Placeable
    {
        [SerializeField] private LayerMask layerMask;

        protected virtual void OnEnable()
        {
            BossPhase.DestroyDevices += OnDestroyDevices;
        }

        protected virtual void OnDisable()
        {
            BossPhase.DestroyDevices -= OnDestroyDevices;
        }

        private void OnDestroyDevices()
        {
            transform.DOMoveY(transform.position.y - 15, 1.0f).SetEase(Ease.InSine).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }

        public override bool Place()
        {
            if (GameManager.Instance.InDisallowedRegion(Collider2D.bounds.center, Collider2D.size))
            {
                return false;
            }
            return !CollisionChecker.IsColliding(gameObject, Collider2D.bounds.center, Collider2D.size, layerMask);
        }
        
    }
}