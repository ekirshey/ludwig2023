
using SadBrains.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
namespace SadBrains.UI
{
    public class ItemDisplay : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private Placeable itemPrefab;
        
        private Placeable _currentItem;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _currentItem = Instantiate(itemPrefab);
            _currentItem.gameObject.SetActive(false);
            _currentItem.StartMove();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _currentItem.EndMove();
            if (!_currentItem.Place())
            {
                _currentItem.Destroy();
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
            itemPrefab = placeable;
        }
    }
}