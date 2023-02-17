using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class Hatch : MonoBehaviour
    {
        [SerializeField] private float retractDistance;
        
        private bool _retracted;
        private float _startX;

        private void Awake()
        {
            _startX = transform.position.x;
        }
        
        private void ToggleRetract()
        {
            transform.DOMoveX(!_retracted ? _startX+retractDistance : _startX, 0.5f);

            _retracted = !_retracted;
        }
        
        private void OnMouseUp()
        {
            ToggleRetract();
        }


    }
}