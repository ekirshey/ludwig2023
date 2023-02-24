using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private GameObject fillPhasePanel;
        [SerializeField] private Button skip;
        [SerializeField] private Timer timer;

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
            Input.DeliveredGoodOutputObject += OnDeliveredGoodOutputObject;
            fillPhasePanel.gameObject.SetActive(true);
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