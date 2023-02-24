using System;
using UnityEngine;

namespace SadBrains
{
    [RequireComponent(typeof(Collider2D))]
    public class OutputObjectTrigger : MonoBehaviour
    {
        public event Action<OutputObject> OutputObjectEntered;

        public Vector3 Center => _collider.bounds.center;

        private Collider2D _collider;
        
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var outputObject = col.GetComponent<OutputObject>();
            if (outputObject == null) return;
            OutputObjectEntered?.Invoke(outputObject);
        }
    }
}