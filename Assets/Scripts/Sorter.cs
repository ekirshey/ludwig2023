using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class Sorter : Device
    {
        [SerializeField] private CootsTrigger topSort;
        [SerializeField] private CootsTrigger bottomSort;
        [SerializeField] private SorterTypeSelect typeSelect;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private ConveyorBelt topConveyor;
        [SerializeField] private ConveyorBelt bottomConveyor;
        [SerializeField] private bool isTopSort;
        
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
            coots.ResetPosition();
            if (coots.CootsType == _allowedCootsType)
            {
                coots.transform.position = isTopSort ? topSort.Center : bottomSort.Center;
            }
            else
            {
                coots.transform.position = isTopSort ? bottomSort.Center : topSort.Center;
            }
        }
        
        private void OnCootsOnBottom(Coots coots)
        {
            coots.ResetPosition();
            if (coots.CootsType == _allowedCootsType)
            {
                coots.transform.position = isTopSort ? topSort.Center : bottomSort.Center;
            }
            else
            {
                coots.transform.position = isTopSort ? bottomSort.Center : topSort.Center;
            }
        }
        
        public override void DisableCollision()
        {
            base.DisableCollision();
            topConveyor.DisableCollision();
            bottomConveyor.DisableCollision();
            spriteRenderer.DOFade(0.5f, 0.1f);
        }

        public override void EnableCollision()
        {
            base.EnableCollision();
            topConveyor.EnableCollision();
            bottomConveyor.EnableCollision();
            spriteRenderer.DOFade(1.0f, 0.1f);
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