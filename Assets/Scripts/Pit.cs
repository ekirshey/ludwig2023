using System;
using UnityEngine;

namespace SadBrains
{
    public class Pit : MonoBehaviour
    {
        public static event Action DestroyedGoodCoots;
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            var coots = col.GetComponent<Coots>();
            if (coots == null) return;

            if (coots.CorrectCoots)
            {
                DestroyedGoodCoots?.Invoke();
            }
            
            Destroy(coots.gameObject);
        }
    }
}