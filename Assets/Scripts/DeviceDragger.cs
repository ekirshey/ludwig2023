using SadBrains.Utils;
using UnityEngine;

namespace SadBrains
{
    public class DeviceDragger : MonoBehaviour
    {
        private Device _device;
        private Vector3 _mousePositionOffset;
        private Vector3 _lastPosition;
        Collider2D[] _hitColliders = new Collider2D[5];
        
        private void Awake()
        {
            _device = GetComponent<Device>();
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
            transform.position = position;
            
            _device.DisableCollision();
        }

        private void OnMouseUp()
        {
            var deviceCollider = _device.Collider2D;

            var numCollisions = Physics2D.OverlapBox(deviceCollider.bounds.center, deviceCollider.size, 0.0f,
                new ContactFilter2D().NoFilter(), _hitColliders);

            _device.EnableCollision();
            
            for (var i = 0; i < numCollisions; i++)
            {
                var colliderObject = _hitColliders[i].gameObject;
                if (colliderObject == _device.gameObject) continue;
                if(colliderObject.transform.IsChildOf(_device.transform)) continue;
                _device.transform.position = _lastPosition;
                return;
            }
        }
    }
}