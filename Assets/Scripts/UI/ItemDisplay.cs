using System;
using System.Collections.Generic;
using SadBrains.Utils;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
namespace SadBrains.UI
{
    public class ItemDisplay : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Placeable itemPrefab;
        [SerializeField] private int count;

        private Stack<Placeable> _items;
        private Placeable _currentItem;

        private void Awake()
        {
            _items = new Stack<Placeable>();
            for (var i = 0; i < count; i++)
            {
                var device = Instantiate(itemPrefab);
                device.gameObject.SetActive(false);
                _items.Push(device);
            }    
        }

        private void OnEnable()
        {
            Trash.DeletePlaceable += OnDelete;
        }

        private void OnDisable()
        {
            Trash.DeletePlaceable -= OnDelete;
        }

        private void OnDelete(Placeable placeable)
        {
            placeable.gameObject.SetActive(false);
            // TODO this check doesnt seem to work?
            if (placeable == _currentItem)
            {
                _items.Push(placeable);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _currentItem = _items.Pop();
            _currentItem.StartMove();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _currentItem.EndMove();
            if (!_currentItem.Place())
            {
                _currentItem.gameObject.SetActive(false);
                _items.Push(_currentItem);
            }

            if (_items.Count == 0)
            {
                Destroy(gameObject);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            var worldPos = MouseHelpers.MouseWorldPosition();
            if (_currentItem.gameObject.activeSelf == false)
            {
                _currentItem.gameObject.SetActive(true);
            }

            var newPosition = worldPos;
            newPosition = new Vector3(Mathf.Round(newPosition.x), Mathf.Round(newPosition.y));
            var prevPosition = _currentItem.transform.position;
            if (newPosition == prevPosition) return;

            var deviceTransform = _currentItem.transform;
            deviceTransform.position = newPosition;

        }
    }
}