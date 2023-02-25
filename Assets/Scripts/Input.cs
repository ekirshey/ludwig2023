using System;
using UnityEngine;

namespace SadBrains
{
    public class Input : MonoBehaviour
    {
        [SerializeField] private FloatingIcon catHappy;
        [SerializeField] private FloatingIcon catAngry;
        [SerializeField] private Transform iconSpawn;
        
        public static event Action<OutputObjectType> DeliveredBadOutputObject;
        public static event Action<OutputObjectType> DeliveredGoodOutputObject;

        [SerializeField] private OutputObjectType expectedOutputType;

        private void OnTriggerEnter2D(Collider2D col)
        {
            var outputObject = col.GetComponent<OutputObject>();
            if (outputObject == null) return;
            
            
            if (outputObject.OutputObjectType != expectedOutputType)
            {
                var icon = Instantiate(catAngry);
                icon.transform.position = iconSpawn.position;
                DeliveredBadOutputObject?.Invoke(outputObject.OutputObjectType);
            }
            else
            {
                var icon = Instantiate(catHappy);
                icon.transform.position = iconSpawn.position;
                DeliveredGoodOutputObject?.Invoke(expectedOutputType);
            }
            outputObject.Destroy();
        }

    }
}