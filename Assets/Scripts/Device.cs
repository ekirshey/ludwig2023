
using UnityEngine;

namespace SadBrains
{
    public abstract class Device : Placeable
    {
        [SerializeField] private LayerMask layerMask;
        
        public override bool Place()
        {
            return !CollisionChecker.IsColliding(gameObject, Collider2D.bounds.center, Collider2D.size, layerMask);
        }
    }
}