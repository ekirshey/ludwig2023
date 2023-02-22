using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SadBrains
{
    public class CootsOutput : MonoBehaviour
    {
        [SerializeField] private Coots cootsPrefabs;
        [SerializeField] private float spawnRate;
        [SerializeField] private Transform spawnLocation;
        [SerializeField] private int defaultWaitTime;
        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private Image typeImage;
        [SerializeField] private Button skipButton;
        [SerializeField] private Animator animator;
        
        public CootsType CootsType { get; private set; }
        private bool _skip;
        private bool _paused;
        private static readonly int OutputCoots = Animator.StringToHash("OutputCoots");
        
        private void Start()
        {
            countdownText.text = defaultWaitTime.ToString();
            StartCoroutine(Wait());
        }

        private void OnEnable()
        {
            Phase.Pause += OnPause;
            Phase.Resume += OnResume;
            skipButton.onClick.AddListener(OnSkip);
        }

        private void OnDisable()
        {
            Phase.Pause -= OnPause;
            Phase.Resume -= OnResume;
            skipButton.onClick.RemoveListener(OnSkip);
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

        private void OnSkip()
        {
            _skip = true;
        }

        private IEnumerator Wait()
        {
            var count = 0;
            while (count < defaultWaitTime && !_skip)
            {
                count++;
                countdownText.text = (defaultWaitTime - count).ToString();
                yield return new WaitForSeconds(1.0f);
            }
            countdownText.gameObject.SetActive(false);
            StartCoroutine(Spawn());
        }
        
        private IEnumerator Spawn()
        {
            while (true)
            {
                if (!_paused)
                {
                    animator.SetTrigger(OutputCoots);
                    var newCoots = Instantiate(cootsPrefabs, transform);
                    newCoots.transform.position = spawnLocation.position;
                    newCoots.SetType(CootsType);
                }

                yield return new WaitForSeconds(spawnRate);
            }
        }

        public void SetCootsType(CootsType cootsType)
        {
            CootsType = cootsType;
            typeImage.sprite = cootsType.sprite;
        }
    }
}