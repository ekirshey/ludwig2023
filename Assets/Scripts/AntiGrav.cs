using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class AntiGrav : Device
    {
        [SerializeField] private OutputObjectTrigger top;
        [SerializeField] private OutputObjectTrigger bottom;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            bottom.OutputObjectEntered += OnOutputObjectEntered;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            bottom.OutputObjectEntered -= OnOutputObjectEntered;
        }

        private void OnOutputObjectEntered(OutputObject outputObject)
        {
            outputObject.DisableRigidBody();
            var sequence = DOTween.Sequence();
            sequence.Append(outputObject.transform.DOMove(bottom.Center, 0.1f))
                .Append(outputObject.transform.DOMove(top.Center, 1.0f))
                .Append(outputObject.transform.DOMoveX(outputObject.transform.position.x + 3, 0.1f))
                .AppendCallback(outputObject.EnableRigidBody).Play();
            
        }

    }
}