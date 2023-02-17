using System;
using UnityEngine;

namespace SadBrains
{
    public class DirectionSwitcher : MonoBehaviour
    {
        [SerializeField] private ConveyorBelt conveyorBelt;
        [SerializeField] private Sprite right;
        [SerializeField] private Sprite left;
        [SerializeField] private SpriteRenderer iconRenderer;
        
        private void Awake()
        {
            iconRenderer.sprite = right;
        }

        void OnMouseUp()
        {
            conveyorBelt.FlipDirection();
            iconRenderer.sprite = iconRenderer.sprite == right ? left : right;
        }
    }
}
