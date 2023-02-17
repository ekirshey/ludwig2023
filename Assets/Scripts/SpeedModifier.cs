using UnityEngine;

namespace SadBrains
{
    public class SpeedModifier : MonoBehaviour
    {
        [SerializeField] private ConveyorBelt conveyorBelt;
    
        void OnMouseUp()
        {
            conveyorBelt.IncrementSpeed();
        }
    }
}