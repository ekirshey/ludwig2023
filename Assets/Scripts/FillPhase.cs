using System.Collections;
using SadBrains.Utils;
using UnityEngine;

namespace SadBrains
{
    public class FillPhase : Phase
    {
        [SerializeField] private CootsOutput outputPrefab;
        [SerializeField] private CootsInput inputPrefab;
        [SerializeField] private float ioSpawnRate;
        
        private void CreateIO()
        {
            var side = Random.Range(0, 2);

            Vector3 outputLoc;
            Vector3 inputLoc;
            side = 0;
            if (side == 0)
            {
                outputLoc = AvailableLeftIOSpawns.PopRandomElement();
                inputLoc = AvailableRightIOSpawns.PopRandomElement();
            }
            else
            {
                outputLoc = AvailableRightIOSpawns.PopRandomElement();
                inputLoc = AvailableLeftIOSpawns.PopRandomElement();
            }

            var cootsType = AvailableCootsTypes.PopRandomElement();
            var output = Instantiate(outputPrefab, GameManager.Instance.transform);
            output.transform.position = outputLoc;
            output.SetCootsType(cootsType);
            
            var input = Instantiate(inputPrefab, GameManager.Instance.transform);
            input.transform.position = inputLoc;
            input.SetCootsType(cootsType);
        }
        
        private IEnumerator IOTimer()
        {
            while (AvailableCootsTypes.Count > 0)
            {
                CreateIO();
                yield return new WaitForSeconds(ioSpawnRate);
            }
        }
        
        public override void SetActive()
        {
            base.SetActive();
         
            StartCoroutine(IOTimer());
        }

        public override void OnDeliveredBadCoots(CootsType received, CootsType expected)
        {
            
        }
        
        public override void OnDeliveredGoodCoots(CootsType type)
        {
            
        }


    }
}