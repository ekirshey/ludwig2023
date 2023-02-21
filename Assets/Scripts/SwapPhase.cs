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
        [SerializeField] private int swapTime;
        [SerializeField] private int numSwaps;
        [SerializeField] private int happinessToContinue;
        // AI gets angry and then you have to build up happiness again
        [SerializeField] private int happinessLoss;

        public override void SetActive()
        {
            base.SetActive();
            StartCoroutine(SwapEvent());
        }

        private IEnumerator SwapEvent()
        {
            DisableDevices();
            yield return new WaitForSeconds(2.0f);

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

            EnableDevices();
        }
        
        
        protected override void OnTimerFinished()
        {
            base.OnTimerFinished();
        }
        
        
    }
}