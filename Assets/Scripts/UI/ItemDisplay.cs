using System.Collections.Generic;
using SadBrains.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
namespace SadBrains.UI
{
    public class ItemDisplay : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Placeable itemPrefab;
        [SerializeField] private int count;

        private Stack<Placeable> Items { get; set; }
        private Placeable _currentItem;

        private void Init()
        {
            Items = new Stack<Placeable>();
            for (var i = 0; i < count; i++)
            {
                var device = Instantiate(itemPrefab);
                device.gameObject.SetActive(false);
                Items.Push(device);
            }    
        }
        
        private void Awake()
        {
            Init();
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
                Items.Push(placeable);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _currentItem = Items.Pop();
            _currentItem.StartMove();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _currentItem.EndMove();
            if (!_currentItem.Place())
            {
                _currentItem.gameObject.SetActive(false);
                Items.Push(_currentItem);
            }

            if (Items.Count == 0)
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

        public void ChangeType(Placeable placeable)
        {
            foreach (var item in Items)
            {
                Destroy(item.gameObject);
            }
            Items.Clear();
            itemPrefab = placeable;
            Init();
        }
    }
}