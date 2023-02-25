using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class TutorialLevel : MonoBehaviour
    {
        [SerializeField] private Output output;
        [SerializeField] private Input input;
        [SerializeField] private int ioEnterSpeed;
        [SerializeField] private int ioEnterDistance;

        public IEnumerator InitializeIO()
        {
            var enter = DOTween.Sequence();
            enter.Join(output.transform.DOMoveX(output.transform.position.x + ioEnterDistance, ioEnterSpeed));
            enter.Join(input.transform.DOMoveX(input.transform.position.x - ioEnterDistance, ioEnterSpeed));
            
            yield return enter.Play().WaitForCompletion();
            output.Initialize(0);
            
        }
        
        public IEnumerator RemoveIO()
        {
            var enter = DOTween.Sequence();
            enter.Join(output.transform.DOMoveX(output.transform.position.x - ioEnterDistance, ioEnterSpeed));
            enter.Join(input.transform.DOMoveX(input.transform.position.x + ioEnterDistance, ioEnterSpeed));
            
            yield return enter.Play().WaitForCompletion();
            Destroy(output.gameObject);
            Destroy(input.gameObject);
        }
    }
}