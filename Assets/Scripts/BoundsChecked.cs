using UnityEngine;

namespace SadBrains
{
    public class BoundsChecked : MonoBehaviour
    {
        [SerializeField] private int minY;
        
        private void Update()
        {
            if (transform.position.y < minY)
            {
                Destroy(gameObject);
            }    
        }
    }
}