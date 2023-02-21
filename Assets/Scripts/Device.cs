
using UnityEngine;

namespace SadBrains
{
    public abstract class Device : Placeable
    {
        [SerializeField] private LayerMask layerMask;

        private void OnEnable()
        {
            Phase.DevicesDisabled += OnDisableDevice;
            Phase.DevicesEnabled += OnEnableDevice;
        }
        
        private void OnDisable()
        {
            Phase.DevicesDisabled -= OnDisableDevice;
            Phase.DevicesEnabled -= OnEnableDevice;
        }

        protected abstract void OnDisableDevice();
        protected abstract void OnEnableDevice();

        public override bool Place()
        {
            return !CollisionChecker.IsColliding(gameObject, Collider2D.bounds.center, Collider2D.size, layerMask);
        }
    }
}