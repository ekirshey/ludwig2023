using System.Collections.Generic;
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
        
        private SurfaceEffector2D _surfaceEffector;
        private float _currentSpeed;
        private int _currentModIdx;
        private int _direction;
        
        private void SetSpeed()
        {
            _currentSpeed = baseSpeed * SpeedMod * _direction;
            _surfaceEffector.speed = _currentSpeed;
        }
        
        private void Start()
        {
            _surfaceEffector = GetComponent<SurfaceEffector2D>();
            _currentModIdx = 0;
            _direction = 1;
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
        
        public override void DisableCollision()
        {
            base.DisableCollision();
            var color = spriteRenderer.color;
            color.a *= 0.5f;
            spriteRenderer.color = color;
        }

        public override void EnableCollision()
        {
            base.EnableCollision();
            var color = spriteRenderer.color;
            color.a *= 2f;
            spriteRenderer.color = color;
        }

        public override void Destroy()
        {
            Destroy(gameObject);
        }
        
    }
}
