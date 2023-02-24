using System;
using UnityEngine;

namespace SadBrains
{
    public class FishInput : MonoBehaviour
    {
        [SerializeField] private FishBomb.FishType expectedFishType;
        
        public static event Action<FishBomb.FishType> DeliveredGoodFish;

        private void OnTriggerEnter2D(Collider2D col)
        {
            var fishBomb = col.GetComponent<FishBomb>();
            if (fishBomb == null) return;

            if (fishBomb.fishType == expectedFishType)
            {
                DeliveredGoodFish?.Invoke(expectedFishType);
            }
            Destroy(fishBomb.gameObject);
        }
    }
}