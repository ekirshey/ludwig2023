using System;
using UnityEngine;
using UnityEngine.UI;

namespace SadBrains
{
    public class CootsInput : MonoBehaviour
    {
        [SerializeField] private Image typeImage;
        
        public static event Action DeliveredBadCoots;
        public static event Action DeliveredGoodCoots;
        
        private CootsType _expectedCootsType;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            var coots = col.GetComponent<Coots>();
            if (coots == null) return;
            
            
            if (coots.CootsType != _expectedCootsType)
            {
                DeliveredBadCoots?.Invoke();
            }
            else
            {
                DeliveredGoodCoots?.Invoke();
            }
            Destroy(coots.gameObject);
        }

        public void SetCootsType(CootsType cootsType)
        {
            _expectedCootsType = cootsType;
            typeImage.sprite = cootsType.sprite;
        }
    }
}