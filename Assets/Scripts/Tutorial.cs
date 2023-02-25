using System;
using System.Collections;
using System.Collections.Generic;
using SadBrains.UI;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField] private GameObject tutorialUI;
        [SerializeField] private TutorialLevel level;
        [SerializeField] private Button endTutorial;
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
                yield return StartCoroutine( level.InitializeIO());
                endTutorial.onClick.AddListener(() =>
                {
                    StartCoroutine(CleanUp());
                });
                tutorialUI.gameObject.SetActive(true);
            }
            else
            {
                StartCoroutine(ExecuteStep(steps[_currentStepIdx]));
            }
            
        }

        private IEnumerator CleanUp()
        {
            tutorialUI.gameObject.SetActive(false);
            CleanupDevices();
            yield return StartCoroutine(level.RemoveIO());
            Finish();
        }
            
        public override void SetActive()
        {
            base.SetActive();
            _currentStepIdx = 0;

            StartCoroutine(ExecuteStep(steps[_currentStepIdx]));
        }
        
    }
}