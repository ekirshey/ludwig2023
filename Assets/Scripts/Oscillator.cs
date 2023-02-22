using UnityEngine;

namespace SadBrains
{
    public class Oscillator : MonoBehaviour
    {
        [SerializeField] private float frequency;
        [SerializeField] private float magnitude;
        [SerializeField] private float offset;

        private bool _enabled;
        private Vector3 _origin;

        private void Start()
        {
            _origin = transform.position;
        }
        
        private void Update()
        {
            if (!_enabled) return;
            var newPosition = _origin + transform.up * (Mathf.Sin(Time.time * frequency + offset) * magnitude);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            transform.position = newPosition;
        }

        public void Disable()
        {
            _enabled = false;
        }

        public void Enable()
        {
            _origin = transform.position;
            _enabled = true;
        }
    }
}