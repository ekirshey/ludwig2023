using System;
using UnityEngine;
using UnityEngine.UI;

namespace SadBrains
{
    public class CootsInput : MonoBehaviour
    {
        [SerializeField] private Image typeImage;
        
        public static event Action<CootsType, CootsType> DeliveredBadCoots;
        public static event Action<CootsType> DeliveredGoodCoots;
        
        private CootsType _expectedCootsType;

        private void OnTriggerEnter2D(Collider2D col)
        {
            var coots = col.GetComponent<Coots>();
            if (coots == null) return;

            if (coots.CootsType != _expectedCootsType)
            {
                DeliveredBadCoots?.Invoke(coots.CootsType, _expectedCootsType);
            }
            else
            {
                DeliveredGoodCoots?.Invoke(_expectedCootsType);
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