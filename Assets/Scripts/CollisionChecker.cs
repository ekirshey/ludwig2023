using System.Collections.Generic;
using UnityEngine;

namespace SadBrains
{
    public static class CollisionChecker
    {
        private static Collider2D[] _hitColliders = new Collider2D[5];

        public static bool IsColliding(GameObject gameObject, Vector2 center, Vector2 size, LayerMask layerMask)
        {
            var contactFilter = new ContactFilter2D
            {
                useLayerMask = true
            };
            contactFilter.SetLayerMask(layerMask);
            
            var numCollisions = Physics2D.OverlapBox(center, size, 0.0f,
                contactFilter, _hitColliders);

            for (var i = 0; i < numCollisions; i++)
            {
                var colliderObject = _hitColliders[i].gameObject;
                if (colliderObject == gameObject) continue;
                if(colliderObject.transform.IsChildOf(gameObject.transform)) continue;
                return true;
            }
            
            return false;
        }
        
        public static List<GameObject> GetCollisions(GameObject gameObject, Vector2 center, Vector2 size)
        {
            var collisions = new List<GameObject>();
            
            var numCollisions = Physics2D.OverlapBox(center, size, 0.0f,
                new ContactFilter2D().NoFilter(), _hitColliders);

            for (var i = 0; i < numCollisions; i++)
            {
                var colliderObject = _hitColliders[i].gameObject;
                if (colliderObject == gameObject) continue;
                if(colliderObject.transform.IsChildOf(gameObject.transform)) continue;
                collisions.Add(colliderObject);
            }

            return collisions;
        }
    }
}