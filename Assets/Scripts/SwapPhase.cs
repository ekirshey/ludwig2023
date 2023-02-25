using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SadBrains.UI;
using SadBrains.Utils;
using UnityEngine;

namespace SadBrains
{
    public class SwapPhase : Phase
    {
        [Tooltip("Mechanic Numbers")]
        [SerializeField] private int swapTime;
        [SerializeField] private int numSwaps;
        [SerializeField] private int happinessToContinue;
        [SerializeField] private int happinessLoss;
        
        [Tooltip("UI")]
        [SerializeField] private GameObject swapUI;
        [SerializeField] private Timer timer;
        [SerializeField] private HappinessDisplay happinessDisplay;
        
        [Tooltip("CatGPT")]
        [SerializeField] private Vector3 catGptStartPosition;
        [SerializeField] private Vector3 catGptPosition;
        [SerializeField] private float catGptSpeed;
        [SerializeField] private List<string> speech;
        [SerializeField] private ScreenShakeController screenShake;
        [SerializeField] private ScreenShakeController.ScreenShakeParameters shakeParams;
        [SerializeField] private int angerLevel;
        
        private Dictionary<OutputObjectType, int> _cootsTracker;

        private void Awake()
        {
            _cootsTracker = new Dictionary<OutputObjectType, int>
            {
                {OutputObjectType.Birman, 1},
                {OutputObjectType.Bombay, 1},
                {OutputObjectType.Scottish, 1},
                {OutputObjectType.Tabby, 1},
                {OutputObjectType.British, 1}
            };
        }
        
        private void Start()
        {
            swapUI.gameObject.SetActive(false);
        }
        
        private IEnumerator CatGptIntro()
        {
            catGpt.transform.position = catGptStartPosition;
            catGpt.SetAnger(angerLevel);
            yield return StartCoroutine(catGpt.MoveToTarget(catGptPosition, catGptSpeed));
            yield return StartCoroutine(catGpt.Speak(speech));
            yield return StartCoroutine(catGpt.MoveToTarget(catGptStartPosition, catGptSpeed));
        }

        private IEnumerator SwapEvent()
        {
            screenShake.Shake(shakeParams);
            PauseAll();
            yield return StartCoroutine(CatGptIntro());
            catGpt.DeductHappiness(happinessLoss);

            var ioList = new List<Input>();
            ioList.AddRange(GameManager.Instance.Inputs);
            for (var i = 0; i < numSwaps; i++)
            {
                var firstInput = ioList.PopRandomElement();
                var secondInput = ioList.PopRandomElement();
                
                var sequence = DOTween.Sequence();
                var firstInitialPos = firstInput.transform.position;
                var secondInitialPos = secondInput.transform.position;
                sequence.Append(firstInput.transform.DOMoveX(firstInitialPos.x + 4, 1.0f))
                    .Join(secondInput.transform.DOMoveX(secondInitialPos.x + 4, 1.0f))
                    .AppendCallback(() =>
                    {
                        var transform1 = firstInput.transform;
                        var transform2 = secondInput.transform;
                        (transform1.position, transform2.position) = (transform2.position, transform1.position);
                    })
                    .AppendInterval(0.5f)
                    .Append(firstInput.transform.DOMove(secondInitialPos, 1.0f))
                    .Join(secondInput.transform.DOMove(firstInitialPos, 1.0f));

                yield return sequence.Play().WaitForCompletion();
                
                ioList.Add(firstInput);
                ioList.Add(secondInput);
            }
            
            swapUI.gameObject.SetActive(true);
            timer.TimerFinished += OnTimerFinished;
            StartCoroutine(timer.RunTimer(swapTime));
        }

        private void OnTimerFinished()
        {
            EnableAll();
            timer.TimerFinished -= OnTimerFinished;
            swapUI.gameObject.SetActive(false);
        }

        private void PhaseLost()
        {
            StartCoroutine(GameLost());
        }

        private void Update()
        {
            if (!Active) return;
            
            // Deliver one of each cat and happiness is above threshold
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
            happinessDisplay.AddTarget(happinessToContinue);
            Input.DeliveredGoodOutputObject += OnDeliveredGoodOutputObject;
            StartCoroutine(SwapEvent());
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