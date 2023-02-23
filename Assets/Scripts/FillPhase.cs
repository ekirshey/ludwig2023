using System.Collections;
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

        private void Awake()
        {
            fillPhasePanel.gameObject.SetActive(false);
            skip.onClick.AddListener(() =>
            {
                timer.ExitTimer();
            });    
        }
        
        private void CreateIO()
        {
            var outputLoc = AvailableLeftIOSpawns.PopRandomElement();
            var inputLoc = AvailableRightIOSpawns.PopRandomElement();

            var cootsType = AvailableCootsTypes.PopRandomElement();
            var output = Instantiate(cootsType.output, GameManager.Instance.transform);
            output.transform.position = outputLoc;
            output.Initialize(outputLoc, cootsType, outputWaitTime);
            
            var input = Instantiate(cootsType.input, GameManager.Instance.transform);
            input.transform.position = inputLoc;
            input.SetCootsType(cootsType);
            
            GameManager.Instance.AddIO(output, input);
        }

        private void FillPhaseFinished()
        {
            fillPhasePanel.gameObject.SetActive(false);
            Finish();
        }
        
        private IEnumerator IOTimer()
        {
            while (AvailableCootsTypes.Count > 0)
            {
                CreateIO();
                yield return StartCoroutine(timer.RunTimer(ioSpawnRate)); ;
            }
            
            // After all io is spawned, set the alert to meet
            catGpt.AddHappinessAlert(FillPhaseFinished, happinessToContinue);
        }
        
        public override void SetActive()
        {
            base.SetActive();
            fillPhasePanel.gameObject.SetActive(true);
            StartCoroutine(IOTimer());
        }





    }
}