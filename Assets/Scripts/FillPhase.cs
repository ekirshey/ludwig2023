using System.Collections;
using SadBrains.Utils;
using UnityEngine;

namespace SadBrains
{
    public class FillPhase : Phase
    {
        [SerializeField] private float ioSpawnRate;
        [SerializeField] private int happinessToContinue;
        
        private void CreateIO()
        {
            var outputLoc = AvailableLeftIOSpawns.PopRandomElement();
            var inputLoc = AvailableRightIOSpawns.PopRandomElement();

            var cootsType = AvailableCootsTypes.PopRandomElement();
            var output = Instantiate(cootsType.output, GameManager.Instance.transform);
            output.transform.position = outputLoc;
            output.SetCootsType(cootsType);
            
            var input = Instantiate(cootsType.input, GameManager.Instance.transform);
            input.transform.position = inputLoc;
            input.SetCootsType(cootsType);
            
            GameManager.Instance.AddIO(output, input);
        }
        
        private IEnumerator IOTimer()
        {
            while (AvailableCootsTypes.Count > 0)
            {
                CreateIO();
                yield return new WaitForSeconds(ioSpawnRate);
            }
            
            // After all io is spawned, set the alert to meet
            catGpt.AddHappinessAlert(Finish, happinessToContinue);
        }
        
        public override void SetActive()
        {
            base.SetActive();
            StartCoroutine(IOTimer());
        }





    }
}