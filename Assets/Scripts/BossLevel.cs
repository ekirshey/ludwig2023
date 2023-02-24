using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class BossLevel : MonoBehaviour
    {
        [SerializeField] private List<FishOutput> outputs;
        [SerializeField] private List<FishInput> inputs;
        [SerializeField] private int ioEnterSpeed;
        [SerializeField] private int ioEnterDistance;
        

        public IEnumerator InitializeIO()
        {
            var enter = DOTween.Sequence();

            foreach (var output in outputs)
            {
                var outputTransform = output.transform;
                enter.Join(outputTransform.DOMoveX(outputTransform.position.x + ioEnterDistance, ioEnterSpeed));
            }
            
            foreach (var input in inputs)
            {
                var inputTransform = input.transform;
                enter.Join(inputTransform.DOMoveX(inputTransform.position.x - ioEnterDistance, ioEnterSpeed));
            }

            yield return enter.Play().WaitForCompletion();
            
            foreach (var output in outputs)
            {
                output.Initialize();
            }
        }

        public IEnumerator DropDebris()
        {
            yield return null;
        }
    }
}