using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SadBrains
{
    public class Output : MonoBehaviour
    {
        [SerializeField] private OutputObject outputObjectPrefab;
        [SerializeField] private float spawnRate;
        [SerializeField] private Transform spawnLocation;
        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private Button skipButton;
        [SerializeField] private Animator animator;
        [SerializeField] private Canvas canvas;
        
        private bool _skip;
        private bool _paused;
        private int _waitTime;
        private static readonly int OutputCoots = Animator.StringToHash("OutputCoots");
        
        private void Start()
        {
            
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
                var outputObject = child.GetComponent<OutputObject>();
                if (outputObject != null)
                {
                    outputObject.DisableCollisions();
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
            canvas.gameObject.SetActive(false);
        }

        private IEnumerator Wait()
        {
            var count = 0;
            while (count < _waitTime && !_skip)
            {
                count++;
                countdownText.text = (_waitTime - count).ToString();
                yield return new WaitForSeconds(1.0f);
            }
            canvas.gameObject.SetActive(false);
            StartCoroutine(Spawn());
        }
        
        private IEnumerator Spawn()
        {
            while (true)
            {
                if (!_paused)
                {
                    animator.SetTrigger(OutputCoots);
                    var newCoots = Instantiate(outputObjectPrefab, transform);
                    newCoots.transform.position = spawnLocation.position;
                }

                yield return new WaitForSeconds(spawnRate);
            }
        }

        public void Initialize(Vector3 position, int waitTime)
        {
            _waitTime = waitTime;
            countdownText.gameObject.SetActive(true);
            countdownText.text = _waitTime.ToString();

            // Tween to position
            var targetPosition = position;
            var initialPosition = position;
            initialPosition.x -= 5;
            transform.position = initialPosition;
            transform.DOMove(targetPosition, 0.5f).OnComplete(() =>
            {
                StartCoroutine(Wait());
            });
        }
        
        public void Initialize(int waitTime)
        {
            _waitTime = waitTime;
            if (_waitTime > 0)
            {
                countdownText.gameObject.SetActive(true);
                countdownText.text = _waitTime.ToString();
            }

            StartCoroutine(Wait());
        }
        
    }
}