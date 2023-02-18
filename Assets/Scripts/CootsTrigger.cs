using System;
using UnityEngine;

namespace SadBrains
{
    [RequireComponent(typeof(Collider2D))]
    public class CootsTrigger : MonoBehaviour
    {
        public event Action<Coots> CootsEntered;

        public Vector3 Center => _collider.bounds.center;

        private Collider2D _collider;
        
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var coots = col.GetComponent<Coots>();
            if (coots == null) return;
            CootsEntered?.Invoke(coots);
        }
    }
}