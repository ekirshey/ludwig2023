using System;
using UnityEngine;

namespace SadBrains
{
    public class Coots : MonoBehaviour
    {
        public bool CorrectCoots { get; private set; }

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetCorrect()
        {
            CorrectCoots = true;
            _spriteRenderer.color = Color.blue;
        }
    }
}