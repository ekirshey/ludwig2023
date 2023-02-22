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
        public event Action<CootsType> CootsTypeChanged;

        private SpriteRenderer _buttonRenderer;
        private BoxCollider2D _boxCollider2D;
        private List<CootsType> _cootsTypes;
        private int _typeIdx;

        private void Start()
        {
            _buttonRenderer = GetComponent<SpriteRenderer>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _cootsTypes = new List<CootsType>();
            _cootsTypes.AddRange(GameManager.Instance.CootsTypes);
            bottomUpHeadshot.sprite = _cootsTypes[_typeIdx].sprite;
            bottomDownHeadshot.sprite = _cootsTypes[_typeIdx].sprite;
            
            CootsTypeChanged?.Invoke(_cootsTypes[_typeIdx]);
        }

        private bool HandleMouseDown()
        {
            if (!Input.GetMouseButtonDown(0) || bottomDown.activeSelf) return false;
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
            
            if (!Input.GetMouseButtonUp(0)) return;
            bottomUp.gameObject.SetActive(true);
            bottomDown.gameObject.SetActive(false);
            _buttonRenderer.sprite = baseSprite;
            var worldPos = MouseHelpers.MouseWorldPosition();

            if (!_boxCollider2D.bounds.Contains(new Vector3(worldPos.x, worldPos.y, 0))) return;
            
            _typeIdx++;
            if (_typeIdx >= _cootsTypes.Count)
            {
                _typeIdx = 0;
            }

            var cootsType = _cootsTypes[_typeIdx];
            bottomUpHeadshot.sprite = _cootsTypes[_typeIdx].sprite;
            bottomDownHeadshot.sprite = _cootsTypes[_typeIdx].sprite;
            CootsTypeChanged?.Invoke(cootsType);
        }
        

    }
}