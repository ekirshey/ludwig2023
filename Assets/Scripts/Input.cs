using System;
using UnityEngine;

namespace SadBrains
{
    public class Input : MonoBehaviour
    {
        public static event Action<OutputObjectType, OutputObjectType> DeliveredBadOutputObject;
        public static event Action<OutputObjectType> DeliveredGoodOutputObject;

        [SerializeField] private OutputObjectType expectedOutputType;

        private void OnTriggerEnter2D(Collider2D col)
        {
            var outputObject = col.GetComponent<OutputObject>();
            if (outputObject == null) return;

            if (outputObject.OutputObjectType != expectedOutputType)
            {
                DeliveredBadOutputObject?.Invoke(outputObject.OutputObjectType, expectedOutputType);
            }
            else
            {
                DeliveredGoodOutputObject?.Invoke(expectedOutputType);
            }
            outputObject.Destroy();
        }

    }
}