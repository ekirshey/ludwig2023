using UnityEngine;

namespace SadBrains
{
    public abstract class Device : MonoBehaviour
    {
        public int width;
        public int height;
        
        public BoxCollider2D Collider2D { get; private set; }

        protected virtual void Awake()
        {
            Collider2D = GetComponent<BoxCollider2D>();
        }

        public virtual void DisableCollision()
        {
            Collider2D.enabled = false;
        }

        public virtual void EnableCollision()
        {
            Collider2D.enabled = true;
        }
    }
}