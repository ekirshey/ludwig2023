using System;
using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class OutputObject : MonoBehaviour
    {
        public event Action Destroyed;
        public static event Action<OutputObjectType> OutputObjectOffScreen;
        [SerializeField] private OutputObjectType type;

        public OutputObjectType OutputObjectType => type;
        public Rigidbody2D Rigidbody2D { get; private set; }
        private BoxCollider2D _collider;
        private Camera _camera;
        private Tween _moveTween;
        
        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            Rigidbody2D = GetComponent<Rigidbody2D>();
            _camera = Camera.main;
        }
        
        private void Update()
        {
            var screenPos = _camera.WorldToScreenPoint(transform.position);
            var onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f && screenPos.y < Screen.height;
            if (onScreen) return;
            OutputObjectOffScreen?.Invoke(type);
            Destroy();
        }
        
        public void ResetVelocity()
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            Rigidbody2D.angularVelocity = 0.0f;
            Rigidbody2D.velocity = new Vector2();
        }
        
        public void DisableRigidBody()
        {
            Rigidbody2D.isKinematic = true;
        }

        public void EnableRigidBody()
        {
            Rigidbody2D.isKinematic = false;
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
            Destroyed?.Invoke();
            _moveTween?.Kill();
            Destroy(gameObject);
        }
        

    }
}