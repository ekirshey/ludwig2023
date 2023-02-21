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
        
        public CootsType ExpectedCootsType { get; private set; }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var coots = col.GetComponent<Coots>();
            if (coots == null) return;

            if (coots.CootsType != ExpectedCootsType)
            {
                DeliveredBadCoots?.Invoke(coots.CootsType, ExpectedCootsType);
            }
            else
            {
                DeliveredGoodCoots?.Invoke(ExpectedCootsType);
            }
            Destroy(coots.gameObject);
        }

        public void SetCootsType(CootsType cootsType)
        {
            ExpectedCootsType = cootsType;
            typeImage.sprite = cootsType.sprite;
        }
    }
}