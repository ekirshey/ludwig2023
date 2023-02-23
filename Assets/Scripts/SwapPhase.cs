using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SadBrains.Utils;
using UnityEngine;

namespace SadBrains
{
    public class SwapPhase : Phase
    {
        [SerializeField] private GameObject swapUI;
        [SerializeField] private Timer timer;
        [SerializeField] private int swapTime;
        [SerializeField] private int numSwaps;
        [SerializeField] private int happinessToContinue;
        [SerializeField] private int happinessLoss;
        [SerializeField] private Vector3 catGptStartPosition;
        [SerializeField] private Vector3 catGptPosition;
        [SerializeField] private float catGptSpeed;
        [SerializeField] private List<string> speech;


        private void Start()
        {
            swapUI.gameObject.SetActive(false);
        }
        
        public override void SetActive()
        {
            base.SetActive();
            StartCoroutine(SwapEvent());
        }

        private IEnumerator CatGptIntro()
        {
            catGpt.transform.position = catGptStartPosition;
            yield return StartCoroutine(catGpt.MoveToTarget(catGptPosition, catGptSpeed));
            foreach (var text in speech)
            {
                yield return StartCoroutine(catGpt.Speak(text));
            }
            yield return StartCoroutine(catGpt.MoveToTarget(catGptStartPosition, catGptSpeed));
        }

        private IEnumerator SwapEvent()
        {
            PauseAll();
            // ADD AI COnversation
            yield return StartCoroutine(CatGptIntro());
            catGpt.DeductHappiness(happinessLoss);

            var ioList = new List<Tuple<CootsOutput, CootsInput>>();
            ioList.AddRange(GameManager.Instance.IoPair);
            for (var i = 0; i < numSwaps; i++)
            {
                var first = ioList.PopRandomElement();
                var second = ioList.PopRandomElement();
                
                var firstOutput = first.Item2;
                var secondOutput = second.Item2;
                var sequence = DOTween.Sequence();
                var firstInitialPos = firstOutput.transform.position;
                var secondInitialPos = secondOutput.transform.position;
                sequence.Append(firstOutput.transform.DOMoveX(firstInitialPos.x + 4, 1.0f))
                    .Join(secondOutput.transform.DOMoveX(secondInitialPos.x + 4, 1.0f))
                    .AppendCallback(() =>
                    {
                        var transform1 = firstOutput.transform;
                        var transform2 = secondOutput.transform;
                        (transform1.position, transform2.position) = (transform2.position, transform1.position);
                    })
                    .AppendInterval(0.5f)
                    .Append(firstOutput.transform.DOMove(secondInitialPos, 1.0f))
                    .Join(secondOutput.transform.DOMove(firstInitialPos, 1.0f));

                yield return sequence.Play().WaitForCompletion();
                
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
        
        
    }
}