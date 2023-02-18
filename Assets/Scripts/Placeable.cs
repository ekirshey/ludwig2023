using System;
using SadBrains.Utils;
using UnityEngine;

namespace SadBrains
{
    public abstract class Placeable : MonoBehaviour
    {
        public event Action MoveStart;
        
        protected BoxCollider2D Collider2D { get; set; }

        public bool IsMoving { get; private set; }
        
        private Vector3 _mousePositionOffset;
        private Vector3 _lastPosition;
        
        protected virtual void Awake()
        {
            Collider2D = GetComponent<BoxCollider2D>();
        }
        
        protected virtual void DisableCollision()
        {
            Collider2D.enabled = false;
        }

        protected virtual void EnableCollision()
        {
            Collider2D.enabled = true;
        }
        
        private void OnMouseDown()
        {
            var position = gameObject.transform.position;
            _mousePositionOffset = position - MouseHelpers.MouseWorldPosition();
            _lastPosition = position;
        }

        private void OnMouseDrag()
        {
            var position = MouseHelpers.MouseWorldPosition() + _mousePositionOffset;
            position = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y));
            if (position == transform.position) return;
            StartMove();
            transform.position = position;
        }

        protected virtual void OnMouseUp()
        {
            EndMove();
            if (!Place())
            {
                transform.position = _lastPosition;
            }
        }

        public void StartMove()
        {
            if (IsMoving) return;
            IsMoving = true;
            MoveStart?.Invoke();
            DisableCollision();
        }

        public void EndMove()
        {
            if (IsMoving)
            {
                IsMoving = false;
                EnableCollision();
            }
        }
        
        public abstract bool Place();
    }
}