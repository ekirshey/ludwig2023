using System;
using UnityEngine;

namespace SadBrains
{
    public class DeliveryTube : MonoBehaviour
    {
        [SerializeField] private CootsType cootsType;
        public static event Action DeliveredBadCoots;
        public static event Action DeliveredGoodCoots;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            var coots = col.GetComponent<Coots>();
            if (coots == null) return;
            
            
            if (coots.CootsType != cootsType)
            {
                DeliveredBadCoots?.Invoke();
            }
            else
            {
                DeliveredGoodCoots?.Invoke();
            }
            Destroy(coots.gameObject);
        }

    }
}