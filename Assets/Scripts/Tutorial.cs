using System;
using System.Collections;
using System.Collections.Generic;
using SadBrains.UI;
using UnityEngine;

namespace SadBrains
{
    public class Tutorial : Phase
    {
        [Serializable]
        public class CatGptStep 
        {
            public Vector3 catGptStart;
            public Vector3 catGptEnd;
            public float moveDuration;
            public BlinkableUI blinkableUI;
            [TextArea(3,10)]
            public string speech;
        }
        
        [SerializeField] private List<CatGptStep> steps;

        private int _currentStepIdx;
        
        private IEnumerator ExecuteStep(CatGptStep step)
        {
            catGpt.transform.position = step.catGptStart;
            yield return StartCoroutine(catGpt.MoveToTarget(step.catGptEnd, step.moveDuration));

            if (step.blinkableUI != null)
            {
                step.blinkableUI.Blink();
            }

            if (step.speech.Length > 0)
            {
                yield return StartCoroutine(catGpt.Speak(step.speech));
            }

            if (step.blinkableUI != null)
            {
                step.blinkableUI.EndBlink();
            }
            
            _currentStepIdx++;
            if (_currentStepIdx >= steps.Count)
            {
                Finish();
            }
            else
            {
                StartCoroutine(ExecuteStep(steps[_currentStepIdx]));
            }
            
        }
            
        public override void SetActive()
        {
            base.SetActive();
            _currentStepIdx = 0;

            StartCoroutine(ExecuteStep(steps[_currentStepIdx]));
        }
        
    }
}