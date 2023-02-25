using System.Collections;
using System.Collections.Generic;
using SadBrains.Utils;
using UnityEngine;

namespace SadBrains.MainMenu
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private int rate;
        [SerializeField] private List<GameObject> cats;
        
        private void Start()
        {
            StartCoroutine(Drop());
        }

        private IEnumerator Drop()
        {
            while (true)
            {
                var cat = Instantiate(cats.RandomElement());
                cat.transform.position = transform.position;
                yield return new WaitForSeconds(rate);
            }
        }
    }
}