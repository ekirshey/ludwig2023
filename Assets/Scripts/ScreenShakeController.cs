using System;
using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class ScreenShakeController : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [Serializable]
        public struct ScreenShakeParameters
        {
            public float duration;
            public float strength;
            public int vibrato;
            public float randomness;
        }
        
        public void Shake(ScreenShakeParameters parameters)
        {
            camera.DOShakePosition(parameters.duration, parameters.strength, parameters.vibrato, parameters.randomness);
        }
    }
}