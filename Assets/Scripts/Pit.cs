using System;
using UnityEngine;

namespace SadBrains
{
    public class Pit : MonoBehaviour
    {
        [SerializeField] private CootsType cootsType;
        
        public static event Action DestroyedGoodCoots;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            var coots = col.GetComponent<Coots>();
            if (coots == null) return;

            if (coots.CootsType == cootsType)
            {
                DestroyedGoodCoots?.Invoke();
            }
            
            Destroy(coots.gameObject);
        }
    }
}