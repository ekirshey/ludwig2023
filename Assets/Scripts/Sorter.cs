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
        [SerializeField] private bool isTopSort;
        [SerializeField] private Transform topTarget;
        [SerializeField] private Transform bottomTarget;
        [SerializeField] private float moveThroughTime;
        
        private CootsType _allowedCootsType;
        private Tween _moveTween;
        
        private void OnEnable()
        {
            typeSelect.CootsTypeChanged += OnCootsTypeChanged;
            topSort.CootsEntered += MoveCoots;
            bottomSort.CootsEntered += MoveCoots;
        }

        private void OnDisable()
        {
            typeSelect.CootsTypeChanged -= OnCootsTypeChanged;
            topSort.CootsEntered -= MoveCoots;
            bottomSort.CootsEntered -= MoveCoots;
        }
        
        private void OnCootsTypeChanged(CootsType type)
        {
            _allowedCootsType = type;
        }

        private void MoveCoots(Coots coots)
        {
            Vector3 targetPosition;
            coots.ResetVelocity();
            
            if (coots.CootsType == _allowedCootsType)
            {
                targetPosition = isTopSort ? topSort.Center : bottomSort.Center;
                targetPosition.x = isTopSort ? topTarget.position.x : bottomTarget.position.x;
            }
            else
            {
                targetPosition = isTopSort ? bottomSort.Center : topSort.Center;
                targetPosition.x = isTopSort ? bottomTarget.position.x : topTarget.position.x;
            }

            coots.transform.position = targetPosition;
        }
        
        
        public override void DisableCollision()
        {
            base.DisableCollision();
            spriteRenderer.DOFade(0.5f, 0.1f);
        }

        public override void EnableCollision()
        {
            base.EnableCollision();
            spriteRenderer.DOFade(1.0f, 0.1f);
        }
    }
}