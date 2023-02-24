using System;
using System.Collections.Generic;
using SadBrains.Utils;
using UnityEngine;

namespace SadBrains
{
    public class SorterTypeSelect : MonoBehaviour
    {
        [SerializeField] private Sprite baseSprite;
        [SerializeField] private Sprite onClickSprite;
        [SerializeField] private GameObject bottomUp;
        [SerializeField] private SpriteRenderer bottomUpHeadshot;
        [SerializeField] private GameObject bottomDown;
        [SerializeField] private SpriteRenderer bottomDownHeadshot;
        [SerializeField] private List<Sprite> sprites;
        [SerializeField] private OutputObjectType minIdx;
        [SerializeField] private OutputObjectType maxIdx;
        public event Action<OutputObjectType> OutputObjectTypeChanged;

        private SpriteRenderer _buttonRenderer;
        private BoxCollider2D _boxCollider2D;
        private int _typeIdx;

        private void Start()
        {
            _buttonRenderer = GetComponent<SpriteRenderer>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            bottomUpHeadshot.sprite = sprites[_typeIdx];
            bottomDownHeadshot.sprite = sprites[_typeIdx];

            OutputObjectTypeChanged?.Invoke(minIdx + _typeIdx);
        }
        
        private bool HandleMouseDown()
        {
            if (!UnityEngine.Input.GetMouseButtonDown(0) || bottomDown.activeSelf) return false;
            var worldPos = MouseHelpers.MouseWorldPosition();
            if (!_boxCollider2D.bounds.Contains(new Vector3(worldPos.x, worldPos.y, 0))) return false;
            _buttonRenderer.sprite = onClickSprite;
            bottomUp.gameObject.SetActive(false);
            bottomDown.gameObject.SetActive(true);
            return true;
        }
        
        private void Update()
        {
            if (HandleMouseDown()) return;
            
            if (!UnityEngine.Input.GetMouseButtonUp(0)) return;
            bottomUp.gameObject.SetActive(true);
            bottomDown.gameObject.SetActive(false);
            _buttonRenderer.sprite = baseSprite;
            var worldPos = MouseHelpers.MouseWorldPosition();

            if (!_boxCollider2D.bounds.Contains(new Vector3(worldPos.x, worldPos.y, 0))) return;
            
            _typeIdx++;
            if (_typeIdx > (int)maxIdx - (int)minIdx)
            {
                _typeIdx = 0;
            }

            bottomUpHeadshot.sprite = sprites[_typeIdx];
            bottomDownHeadshot.sprite = sprites[_typeIdx];
            OutputObjectTypeChanged?.Invoke(minIdx + _typeIdx);
        }
        

    }
}