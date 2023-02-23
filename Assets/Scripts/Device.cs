
using UnityEngine;

namespace SadBrains
{
    public abstract class Device : Placeable
    {
        [SerializeField] private LayerMask layerMask;
        
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