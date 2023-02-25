using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class AntiGrav : Device
    {
        [SerializeField] private Transform top;
        [SerializeField] private OutputObjectTrigger bottom;
        
        private Sequence _sequence;
        private OutputObject _current;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            bottom.OutputObjectEntered += OnOutputObjectEnteredBottom;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            bottom.OutputObjectEntered -= OnOutputObjectEnteredBottom;
        }

        private void KillSequence()
        {
            _sequence?.Kill();
        }
        
        private void OnOutputObjectEnteredBottom(OutputObject outputObject)
        {
            outputObject.Destroyed += KillSequence;
            outputObject.EnteredTrigger += KillSequence;
            outputObject.ResetVelocity();
            _sequence = DOTween.Sequence();
            _sequence.Append(outputObject.transform.DOMove(bottom.Center, 0.1f))
                .Append(outputObject.transform.DOMove(top.position, 1.0f))
                .Append(outputObject.transform.DOMoveX(outputObject.transform.position.x + 3.5f, 0.1f))
                .AppendCallback(() =>
                {
                    outputObject.EnteredTrigger -= KillSequence;
                    outputObject.Destroyed -= KillSequence;
                }).Play();
        }

        public override void Destroy()
        {
            Destroy(gameObject);
        }

    }
}