using System;
using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class OutputObject : MonoBehaviour
    {
        public static event Action OutputObjectDestroyed;
        [SerializeField] private OutputObjectType type;

        public OutputObjectType OutputObjectType => type;
        private BoxCollider2D _collider;
        private Rigidbody2D _rigidbody2D;
        private Camera _camera;
        private Tween _moveTween;
        
        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _camera = Camera.main;
        }
        
        private void Update()
        {
            var screenPos = _camera.WorldToScreenPoint(transform.position);
            var onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;
            if (onScreen) return;
            Destroy();
        }
        
        public void ResetVelocity()
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            _rigidbody2D.angularVelocity = 0.0f;
            _rigidbody2D.velocity = new Vector2();
        }
        
        public void DisableRigidBody()
        {
            _rigidbody2D.isKinematic = true;
        }

        public void EnableRigidBody()
        {
            _rigidbody2D.isKinematic = false;
        }
        
        public void DisableCollisions()
        {
            _collider.enabled = false;
        }

        public void EnableCollisions()
        {
            _collider.enabled = true;
        }
        
        public void Destroy()
        {
            _moveTween?.Kill();
            Destroy(gameObject);
            OutputObjectDestroyed?.Invoke();
        }
        

    }
}