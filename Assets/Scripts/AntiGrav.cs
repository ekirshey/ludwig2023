using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class AntiGrav : Device
    {
        [SerializeField] private CootsTrigger top;
        [SerializeField] private CootsTrigger bottom;
        
        private void OnEnable()
        {
            top.CootsEntered += OnCootsOnTop;
            bottom.CootsEntered += OnCootsOnBottom;
        }

        private void OnDisable()
        {
            top.CootsEntered -= OnCootsOnTop;
            bottom.CootsEntered -= OnCootsOnBottom;
        }
        
        private void OnCootsOnTop(Coots coots)
        {

        }
        
        private void OnCootsOnBottom(Coots coots)
        {
            coots.DisableRigidBody();
            var sequence = DOTween.Sequence();
            sequence.Append(coots.transform.DOMove(bottom.Center, 0.1f))
                .Append(coots.transform.DOMove(top.Center, 1.0f))
                .Append(coots.transform.DOMoveX(coots.transform.position.x + 3, 0.1f))
                .AppendCallback(coots.EnableRigidBody).Play();
            
        }
        
        public override bool CanReceiveSignal(DeviceSignal signal)
        {
            return true;
        }

        public override void ReceiveSignal(DeviceSignal signal)
        {
            
        }

        public override int GetCurrentSignalState(DeviceSignal signal)
        {
            return 0;
        }
    }
}