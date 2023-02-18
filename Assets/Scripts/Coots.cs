using UnityEngine;

namespace SadBrains
{
    public class Coots : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        public CootsType CootsType { get; private set; }

        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        
        public void SetType(CootsType type)
        {
            CootsType = type;
            spriteRenderer.sprite = type.sprite;
        }

        public void ResetPosition()
        {
            transform.rotation = Quaternion.identity;
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
    }
}