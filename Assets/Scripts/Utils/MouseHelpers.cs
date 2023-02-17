using UnityEngine;
using UnityEngine.InputSystem;

namespace SadBrains.Utils
{
    public static class MouseHelpers
    {
        public static Vector3 MouseWorldPosition()
        {
            var mousePosition = Mouse.current.position.ReadValue();
            var worldPos = Camera.main.ScreenToWorldPoint(mousePosition);

            return worldPos;
        }
    }
}