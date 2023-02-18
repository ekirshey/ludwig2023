using System;
using System.Collections.Generic;
using SadBrains.Utils;
using UnityEngine;

namespace SadBrains
{
    public class SorterTypeSelect : MonoBehaviour
    {
        public event Action<CootsType> CootsTypeChanged;

        private SpriteRenderer _spriteRenderer;
        private BoxCollider2D _boxCollider2D;
        private List<CootsType> _cootsTypes;
        private int _typeIdx;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _cootsTypes = new List<CootsType>();
            _cootsTypes.AddRange(GameManager.Instance.CootsTypes);
            _spriteRenderer.sprite = _cootsTypes[_typeIdx].sprite;
            
            CootsTypeChanged?.Invoke(_cootsTypes[_typeIdx]);
        }
        
        private void Update()
        {
            if (!Input.GetMouseButtonUp(0)) return;

            var worldPos = MouseHelpers.MouseWorldPosition();

            if (!_boxCollider2D.bounds.Contains(new Vector3(worldPos.x, worldPos.y, 0))) return;
            
            _typeIdx++;
            if (_typeIdx >= _cootsTypes.Count)
            {
                _typeIdx = 0;
            }

            var cootsType = _cootsTypes[_typeIdx];
            _spriteRenderer.sprite = cootsType.sprite;
            CootsTypeChanged?.Invoke(cootsType);
        }

    }
}