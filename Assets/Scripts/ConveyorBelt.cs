using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class ConveyorBelt : Device
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float baseSpeed;
        [SerializeField] private List<float> speedMods;
    
        public float SpeedMod => speedMods[_currentModIdx];
        
        private List<SurfaceEffector2D> _surfaceEffectors;
        private float _currentSpeed;
        private int _currentModIdx;
        private int _direction;

        private void SetSpeed()
        {
            _currentSpeed = baseSpeed * SpeedMod * _direction;
            foreach (var surfaceEffector in _surfaceEffectors)
            {
                surfaceEffector.speed = _currentSpeed;
            }
        }
        
        private void Start()
        {
            _surfaceEffectors = GetComponentsInChildren<SurfaceEffector2D>().ToList();
            _currentModIdx = 0;
            _direction = 1;
            SetSpeed();
        }

        private void FlipDirection()
        {
            _direction *= -1;
            SetSpeed();
        }

        public void IncrementSpeed()
        {
            _currentModIdx++;
            if (_currentModIdx >= speedMods.Count)
            {
                _currentModIdx = 0;
            }
            SetSpeed();
        }
        
        protected override void DisableCollision()
        {
            base.DisableCollision();
            spriteRenderer.DOFade(0.5f, 0.1f);
        }

        protected override void EnableCollision()
        {
            base.EnableCollision();
            spriteRenderer.DOFade(1.0f, 0.1f);
        }

        public override bool CanReceiveSignal(DeviceSignal signal)
        {
            if (signal == DeviceSignal.Direction) return true;
            return false;
        }
        
        public override void ReceiveSignal(DeviceSignal signal)
        {
            if (signal != DeviceSignal.Direction) return;
            FlipDirection();
        }

        public override int GetCurrentSignalState(DeviceSignal signal)
        {
            if (signal != DeviceSignal.Direction) return 0;

            return _direction == 1 ? 0 : 1;
        }
        
    }
}
