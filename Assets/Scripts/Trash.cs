using System;
using SadBrains.Utils;
using UnityEngine;

namespace SadBrains
{
    public class Trash : MonoBehaviour
    {
        public static event Action<Placeable> DeletePlaceable;

        [SerializeField] private Sprite hoverSprite;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private Placeable _currentPlaceable;
        private BoxCollider2D _boxCollider2D;
        private Color _defaultColor;
        private Color _hiddenColor = new Color(1, 1, 1, 0);
        private Sprite _baseSprite;
        
        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _defaultColor = spriteRenderer.color;
            _baseSprite = spriteRenderer.sprite;
            spriteRenderer.color = _hiddenColor;
        }

        private void OnEnable()
        {
            Placeable.DragStart += OnDragStart;
            Placeable.DragEnd += OnDragEnd;
        }

        private void OnDisable()
        {
            Placeable.DragStart -= OnDragStart;
            Placeable.DragEnd -= OnDragEnd;
        }

        private void OnDragStart(Placeable placeable)
        {
            _currentPlaceable = placeable;
            spriteRenderer.color = _defaultColor;
        }

        private void OnDragEnd(Placeable placeable)
        {
            if (_currentPlaceable == null) return;

            spriteRenderer.color = _hiddenColor;
            
            var worldPos = MouseHelpers.MouseWorldPosition();

            if (!_boxCollider2D.bounds.Contains(new Vector3(worldPos.x, worldPos.y, 0))) return;
            
            DeletePlaceable?.Invoke(_currentPlaceable);
            _currentPlaceable = null;
        }

        private void Update()
        {
            if (_currentPlaceable == null) return;
            
            var worldPos = MouseHelpers.MouseWorldPosition();
            if (!_boxCollider2D.bounds.Contains(new Vector3(worldPos.x, worldPos.y, 0)))
            {
                spriteRenderer.sprite = _baseSprite;
            }
            else
            {
                spriteRenderer.sprite = hoverSprite;    
            }
            
        }
    }
}