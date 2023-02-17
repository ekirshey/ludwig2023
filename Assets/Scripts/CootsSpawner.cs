using System.Collections;
using UnityEngine;

namespace SadBrains
{
    public class CootsSpawner : MonoBehaviour
    {
        [SerializeField] private Coots cootsPrefabs;
        [SerializeField] private float spawnRate;
        [SerializeField] private int correctCootsChance;
        
        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                var newCoots = Instantiate(cootsPrefabs, transform);
                newCoots.transform.localPosition = new Vector3();
                var chance = Random.Range(1, 101);
                if (chance <= correctCootsChance)
                {
                    newCoots.SetCorrect();
                }
                
                yield return new WaitForSeconds(spawnRate);
            }
        }
    }
}