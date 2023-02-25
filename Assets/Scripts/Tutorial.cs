
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SadBrains
{
    public class Tutorial : Phase
    {
        [SerializeField] private GameObject tutorialUI;
        [SerializeField] private TutorialLevel level;
        [SerializeField] private Button endTutorial;
        [SerializeField] private Vector3 catGptStartPosition;
        [SerializeField] private Vector3 catGptPosition;
        [SerializeField] private float catGptSpeed;
        [TextArea(3,10)]
        [SerializeField] private List<string> speech;

        private IEnumerator RunTutorial()
        {
            catGpt.transform.position = catGptStartPosition;
            yield return StartCoroutine(catGpt.MoveToTarget(catGptPosition, catGptSpeed));
            yield return StartCoroutine(catGpt.Speak(speech));
            yield return StartCoroutine(catGpt.MoveToTarget(catGptStartPosition, catGptSpeed));
            
            yield return StartCoroutine( level.InitializeIO());
            endTutorial.onClick.AddListener(() =>
            {
                StartCoroutine(CleanUp());
            });
            tutorialUI.gameObject.SetActive(true);
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
            catGpt.HappinessLocked = true;
            StartCoroutine(RunTutorial());
        }
        
    }
}