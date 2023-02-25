using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class AntiGrav : Device
    {
        [SerializeField] private OutputObjectTrigger top;
        [SerializeField] private OutputObjectTrigger bottom;
        
        private Sequence _sequence;
        protected override void OnEnable()
        {
            base.OnEnable();
            top.OutputObjectEntered += OnOutputObjectEnteredTop;
            bottom.OutputObjectEntered += OnOutputObjectEnteredBottom;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            top.OutputObjectEntered -= OnOutputObjectEnteredTop;
            bottom.OutputObjectEntered -= OnOutputObjectEnteredBottom;
        }

        private void OnOutputObjectEnteredTop(OutputObject obj)
        {
            
        }

        private void KillSequence()
        {
            _sequence?.Kill();
        }
        
        private void OnOutputObjectEnteredBottom(OutputObject outputObject)
        {
            outputObject.Destroyed += KillSequence;
            outputObject.DisableRigidBody();
            _sequence = DOTween.Sequence();
            _sequence.Append(outputObject.transform.DOMove(bottom.Center, 0.1f))
                .Append(outputObject.transform.DOMove(top.Center, 1.0f))
                .Append(outputObject.transform.DOMoveX(outputObject.transform.position.x + 3.5f, 0.1f))
                .AppendCallback(() =>
                {
                    outputObject.EnableRigidBody();
                    outputObject.Destroyed -= KillSequence;
                }).Play();
        }

        public override void Destroy()
        {
            Destroy(gameObject);
        }

    }
}