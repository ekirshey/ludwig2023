using System.Collections;
using System.Collections.Generic;
using SadBrains.Utils;
using UnityEngine;

namespace SadBrains
{
    public class CatDropper : MonoBehaviour
    {
        [SerializeField] private int y;
        [SerializeField] private int minX;
        [SerializeField] private int maxX;
        [SerializeField] private List<GameObject> cats;
        [SerializeField] private float rate;
        
        private void Start()
        {
            StartCoroutine(Drop());
        }

        private IEnumerator Drop()
        {
            while (true)
            {
                var randomX = Random.Range(minX, maxX);
                var cat = Instantiate(cats.RandomElement());
                cat.transform.position = new Vector3(randomX, y, 0);
                yield return new WaitForSeconds(rate);
            }
        }
    }
}