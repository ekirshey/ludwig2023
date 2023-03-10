using System.Collections;
using System.Collections.Generic;
using SadBrains.UI;
using SadBrains.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace SadBrains
{
    public class FillPhase : Phase
    {
        [SerializeField] private int ioSpawnRate;
        [SerializeField] private int happinessToContinue;
        [SerializeField] private int outputWaitTime;
        [SerializeField] private HappinessDisplay happinessDisplay;
        [SerializeField] private GameObject fillPhasePanel;
        [SerializeField] private Button skip;
        [SerializeField] private Timer timer;
        
        [Tooltip("CatGPT")]
        [SerializeField] private Vector3 catGptStartPosition;
        [SerializeField] private Vector3 catGptPosition;
        [SerializeField] private float catGptSpeed;
        [TextArea(3,10)]
        [SerializeField] private List<string> speech;

        private Dictionary<OutputObjectType, int> _cootsTracker;

        private void Awake()
        {
            fillPhasePanel.gameObject.SetActive(false);
            skip.onClick.AddListener(() =>
            {
                timer.ExitTimer();
            });

            _cootsTracker = new Dictionary<OutputObjectType, int>
            {
                {OutputObjectType.Birman, 1},
                {OutputObjectType.Bombay, 1},
                {OutputObjectType.Scottish, 1},
                {OutputObjectType.Tabby, 1},
                {OutputObjectType.British, 1}
            };
        }
        
        private IEnumerator CatGptIntro()
        {
            catGpt.transform.position = catGptStartPosition;
            yield return StartCoroutine(catGpt.MoveToTarget(catGptPosition, catGptSpeed));
            yield return StartCoroutine(catGpt.Speak(speech));
            yield return StartCoroutine(catGpt.MoveToTarget(catGptStartPosition, catGptSpeed));
        }
        
        private void CreateIO()
        {
            var outputLoc = AvailableLeftIOSpawns.PopRandomElement();
            var inputLoc = AvailableRightIOSpawns.PopRandomElement();

            var cootsType = AvailableIOPairs.PopRandomElement();
            var output = Instantiate(cootsType.output, GameManager.Instance.transform);
            output.transform.position = outputLoc;
            output.Initialize(outputLoc, outputWaitTime);
            
            var input = Instantiate(cootsType.input, GameManager.Instance.transform);
            input.transform.position = inputLoc;
            
            GameManager.Instance.AddIO(output, input);
        }

        private void PhaseLost()
        {
            StartCoroutine(GameLost());
        }

        private IEnumerator IOTimer()
        {
            yield return StartCoroutine(CatGptIntro());
            fillPhasePanel.gameObject.SetActive(true);
            while (true)
            {
                CreateIO();
                if (AvailableIOPairs.Count <= 0)
                {
                    break;
                }
                yield return StartCoroutine(timer.RunTimer(ioSpawnRate));
            }
            fillPhasePanel.gameObject.SetActive(false);
        }
        
        private void Update()
        {
            if (!Active) return;
            // Deliver at least one of each cat before counting happiness
            if (_cootsTracker.Count == 0 && catGpt.Happiness >= happinessToContinue)
            {
                happinessDisplay.ClearTarget();
                Finish();
            }
            else if (catGpt.Happiness <= 0)
            {
                PhaseLost();
            }
        }
        
        public override void SetActive()
        {
            base.SetActive();
            catGpt.HappinessLocked = false;
            happinessDisplay.AddTarget(happinessToContinue);
            Input.DeliveredGoodOutputObject += OnDeliveredGoodOutputObject;
            StartCoroutine(IOTimer());
        }

        private void OnDeliveredGoodOutputObject(OutputObjectType obj)
        {
            if (!Active) return;
            if (!_cootsTracker.ContainsKey(obj)) return;
            _cootsTracker[obj]--;
            if (_cootsTracker[obj] <= 0)
            {
                _cootsTracker.Remove(obj);
            }
        }
    }
}