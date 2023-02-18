using UnityEngine;

namespace SadBrains
{
    public class Sorter : Device
    {
        [SerializeField] private CootsTrigger topSort;
        [SerializeField] private CootsTrigger bottomSort;
        [SerializeField] private SorterTypeSelect typeSelect;
        
        private CootsType _allowedCootsType;
        
        
        private void OnEnable()
        {
            typeSelect.CootsTypeChanged += OnCootsTypeChanged;
            topSort.CootsEntered += OnCootsOnTop;
            bottomSort.CootsEntered += OnCootsOnBottom;
        }

        private void OnDisable()
        {
            typeSelect.CootsTypeChanged -= OnCootsTypeChanged;
            topSort.CootsEntered -= OnCootsOnTop;
            bottomSort.CootsEntered -= OnCootsOnBottom;
        }

        private void OnCootsTypeChanged(CootsType type)
        {
            _allowedCootsType = type;
        }

        private void OnCootsOnTop(Coots coots)
        {
            if (coots.CootsType != _allowedCootsType)
            {
                coots.transform.position = bottomSort.Center;
                coots.ResetPosition();
            }
        }
        
        private void OnCootsOnBottom(Coots coots)
        {
            if (coots.CootsType == _allowedCootsType)
            {
                coots.transform.position = topSort.Center;
                coots.ResetPosition();
            }
        }

        public override bool CanReceiveSignal(DeviceSignal signal)
        {
            return false;
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