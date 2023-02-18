using System.Collections;
using UnityEngine;

namespace SadBrains
{
    public class CootsSpawner : MonoBehaviour
    {
        [SerializeField] private Coots cootsPrefabs;
        [SerializeField] private float spawnRate;
        [SerializeField] private Transform spawnLocation;
        [SerializeField] private CootsType cootsToSpawn;
        
        private void Start()
        {
            StartCoroutine(Spawn());
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                var newCoots = Instantiate(cootsPrefabs, transform);
                newCoots.transform.position = spawnLocation.position;
                newCoots.SetType(cootsToSpawn);
                
                yield return new WaitForSeconds(spawnRate);
            }
        }
    }
}