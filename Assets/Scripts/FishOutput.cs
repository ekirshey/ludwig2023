using System.Collections;
using UnityEngine;

namespace SadBrains
{
    public class FishOutput : MonoBehaviour
    {
        [SerializeField] private FishBomb fishPrefab;
        [SerializeField] private float spawnRate;
        [SerializeField] private Transform spawnLocation;
        [SerializeField] private Animator animator;

        private bool _paused;
        private int _waitTime;
        private static readonly int OutputCoots = Animator.StringToHash("OutputCoots");

        private void OnEnable()
        {
            Phase.Pause += OnPause;
            Phase.Resume += OnResume;
        }

        private void OnDisable()
        {
            Phase.Pause -= OnPause;
            Phase.Resume -= OnResume;
        }

        private void OnPause()
        {
            _paused = true;
            foreach (Transform child in transform)
            {
                var coot = child.GetComponent<Coots>();
                if (coot != null)
                {
                    coot.DisableCollisions();
                }
            }
        }

        private void OnResume()
        {
            _paused = false;
        }

        private IEnumerator Spawn()
        {
            while (true)
            {
                if (!_paused)
                {
                    animator.SetTrigger(OutputCoots);
                    var fish = Instantiate(fishPrefab, transform);
                    fish.transform.position = spawnLocation.position;
                }
                yield return new WaitForSeconds(spawnRate);
            }
        }
        
        public void Initialize()
        {
            StartCoroutine(Spawn());
        }
    }
}