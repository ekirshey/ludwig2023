using UnityEngine;

namespace SadBrains.UI
{
    public class DeviceController : MonoBehaviour
    {
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
            placeable.Destroy();
        }
    }
}