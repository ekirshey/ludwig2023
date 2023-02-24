using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class Sorter : Device
    {
        [SerializeField] private OutputObjectTrigger topSort;
        [SerializeField] private OutputObjectTrigger bottomSort;
        [SerializeField] private SorterTypeSelect typeSelect;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private bool isTopSort;
        [SerializeField] private Transform topTarget;
        [SerializeField] private Transform bottomTarget;
        [SerializeField] private float moveThroughTime;

        private bool _sortFish;
        private CootsType _allowedCootsType;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            typeSelect.CootsTypeChanged += OnCootsTypeChanged;
            topSort.OutputObjectEntered += MoveOutputObject;
            bottomSort.OutputObjectEntered += MoveOutputObject;
            BossPhase.SortFish += SwapToFish;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            typeSelect.CootsTypeChanged -= OnCootsTypeChanged;
            topSort.OutputObjectEntered -= MoveOutputObject;
            bottomSort.OutputObjectEntered -= MoveOutputObject;
            BossPhase.SortFish -= SwapToFish;
        }
        
        private void SwapToFish()
        {
            _sortFish = true;
        }
        
        private void OnCootsTypeChanged(CootsType type)
        {
            _allowedCootsType = type;
        }

        private void MoveOutputObject(OutputObject outputObject)
        {
            outputObject.ResetVelocity();

            if (_sortFish)
            {
                SortFish();
            }
            else
            {
                SortCoots(outputObject);
            }
        }

        private void SortCoots(OutputObject outputObject)
        {
            Vector3 targetPosition;
            
            var coots = outputObject.GetComponent<Coots>();
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

        private void SortFish()
        {
            
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