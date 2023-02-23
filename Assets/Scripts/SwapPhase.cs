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
        [SerializeField] private Timer timer;
        [SerializeField] private int swapTime;
        [SerializeField] private int numSwaps;
        [SerializeField] private int happinessToContinue;
        [SerializeField] private int happinessLoss;
        [SerializeField] private Vector3 catGptStartPosition;
        [SerializeField] private Vector3 catGptPosition;
        [SerializeField] private float catGptSpeed;
        [SerializeField] private List<string> speech;

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

                var newSecond = new Tuple<CootsOutput, CootsInput>(second.Item1, first.Item2);
                var newFirst = new Tuple<CootsOutput, CootsInput>(second.Item1, second.Item2);

                GameManager.Instance.RemoveIO(first);
                GameManager.Instance.RemoveIO(second);
                GameManager.Instance.AddIO(newFirst.Item1, newFirst.Item2);
                GameManager.Instance.AddIO(newSecond.Item1, newSecond.Item2);

                var firstOutput = newFirst.Item2;
                var secondOutput = newSecond.Item2;
                var sequence = DOTween.Sequence();
                var firstInitialPos = firstOutput.transform.position;
                var secondInitialPos = secondOutput.transform.position;
                sequence.Append(firstOutput.transform.DOMoveX(firstInitialPos.x + 4, 1.0f))
                    .Join(secondOutput.transform.DOMoveX(secondInitialPos.x + 4, 1.0f))
                    .AppendCallback(() =>
                    {
                        newSecond.Item2.SetCootsType(second.Item1.CootsType);
                        newFirst.Item2.SetCootsType(first.Item1.CootsType);
                    })
                    .AppendInterval(0.5f)
                    .Append(firstOutput.transform.DOMove(firstInitialPos, 1.0f))
                    .Join(secondOutput.transform.DOMove(secondInitialPos, 1.0f));

                yield return sequence.Play().WaitForCompletion();

            }
            
            timer.TimerFinished += OnTimerFinished;
            StartCoroutine(timer.RunTimer(swapTime));
        }

        private void OnTimerFinished()
        {
            EnableAll();
            timer.TimerFinished -= OnTimerFinished;
        }
        
        
    }
}