using System.Collections.Generic;
using SadBrains.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
namespace SadBrains.UI
{
    public class DeviceDisplay : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Device devicePrefab;
        [SerializeField] private int count;

        private Stack<Device> _devices;
        private Device _currentDevice;
        Collider2D[] _hitColliders = new Collider2D[5];
        
        private void Awake()
        {
            _devices = new Stack<Device>();
            for (var i = 0; i < count; i++)
            {
                var device = Instantiate(devicePrefab);
                device.gameObject.SetActive(false);
                _devices.Push(device);
            }    
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _currentDevice = _devices.Pop();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var deviceCollider = _currentDevice.Collider2D;

            var numCollisions = Physics2D.OverlapBox(deviceCollider.bounds.center, deviceCollider.size, 0.0f,
                new ContactFilter2D().NoFilter(), _hitColliders);

            for (var i = 0; i < numCollisions; i++)
            {
                var colliderObject = _hitColliders[i].gameObject;
                if (colliderObject == _currentDevice.gameObject) continue;
                if(colliderObject.transform.IsChildOf(_currentDevice.transform)) continue;
                _currentDevice.gameObject.SetActive(false);
                _devices.Push(_currentDevice);
                return;
            }

            if (_devices.Count == 0)
            {
                Destroy(gameObject);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            var worldPos = MouseHelpers.MouseWorldPosition();
            if (_currentDevice.gameObject.activeSelf == false)
            {
                _currentDevice.gameObject.SetActive(true);
            }

            var newPosition = worldPos;
            newPosition = new Vector3(Mathf.Round(newPosition.x), Mathf.Round(newPosition.y));
            var prevPosition = _currentDevice.transform.position;
            if (newPosition == prevPosition) return;

            var deviceTransform = _currentDevice.transform;
            deviceTransform.position = newPosition;

        }
    }
}