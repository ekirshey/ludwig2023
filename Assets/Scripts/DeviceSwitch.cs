using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class DeviceSwitch : Placeable
    {
        [SerializeField] private Sprite stateOne;
        [SerializeField] private Sprite stateTwo;
        [SerializeField] private SpriteRenderer iconRenderer;
        [SerializeField] private DeviceSignal signalType;

        private BoxCollider2D _collider;
        private Device _device;
        private Vector3 _deviceLastLocation;
        private int _stateValue;
        
        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<BoxCollider2D>();
            iconRenderer.sprite = stateOne;
        }

        protected override void OnMouseUp()
        {
            if (!IsMoving)
            {
                _stateValue = _stateValue == 0 ? 1 : 0;
                iconRenderer.sprite = _stateValue == 0 ? stateOne : stateTwo;
                _device.ReceiveSignal(signalType);
            }

            base.OnMouseUp();
        }

        private void LinkToDevice(Device device)
        {
            if (_device != null)
            {
                _device.MoveStart -= OnDeviceMove;
            }
            
            _device = device;
            _stateValue = _device.GetCurrentSignalState(signalType);
            iconRenderer.sprite = _stateValue == 0 ? stateOne : stateTwo;
            _deviceLastLocation = _device.transform.position;
            _device.MoveStart += OnDeviceMove;
        }
        
        private void OnDeviceMove()
        {
            StartCoroutine(FollowDevice());
        }
        
        private IEnumerator FollowDevice()
        {
            StartMove();
            while (_device.IsMoving)
            {
                var devicePosition = _device.transform.position;
                var diff = devicePosition - _deviceLastLocation;
                transform.position += diff;
                
                _deviceLastLocation = devicePosition;
                yield return null;
            }
            
            EndMove();
        }
        
        protected override void DisableCollision()
        {
            base.DisableCollision();
            iconRenderer.DOFade(0.5f, 0.1f);
        }

        protected override void EnableCollision()
        {
            base.EnableCollision();
            iconRenderer.DOFade(1.0f, 0.1f);
        }
        
        public override bool Place()
        {
            // first check normal collisions
            if(CollisionChecker.IsColliding(gameObject, Collider2D.bounds.center, Collider2D.size))
            {
                return false;
            }
            
            var collisions = CollisionChecker.GetCollisions(gameObject, _collider.bounds.center, _collider.size * 2);

            if (collisions.Count == 0)
            {
                return false;
            }

            foreach (var obj in collisions)
            {
                var device = obj.GetComponent<Device>();
                if(device == null) continue;
                if (!device.CanReceiveSignal(signalType)) continue;
                LinkToDevice(device);
                return true;
            }
            
            return true;
        }
        
    }
}
