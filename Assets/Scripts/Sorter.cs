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
            coots.DisableRigidBody();
            coots.ResetVelocity();
            Tween moveTween;
            if (coots.CootsType == _allowedCootsType)
            {
                coots.transform.position = isTopSort ? topSort.Center : bottomSort.Center;
                moveTween = coots.transform.DOMoveX(isTopSort ? topTarget.position.x : bottomTarget.position.x, moveThroughTime);
            }
            else
            {
                coots.transform.position = isTopSort ? bottomSort.Center : topSort.Center;
                moveTween = coots.transform.DOMoveX(isTopSort ? bottomTarget.position.x : topTarget.position.x, moveThroughTime);
            }

            moveTween.OnComplete(() =>
            {
                coots.EnableRigidBody();
                coots.ResetVelocity();
            });
        }
        
        private void OnCootsOnBottom(Coots coots)
        {
            coots.DisableRigidBody();
            coots.ResetVelocity();
            Tween moveTween;
            
            if (coots.CootsType == _allowedCootsType)
            {
                coots.transform.position = isTopSort ? topSort.Center : bottomSort.Center;
                moveTween = coots.transform.DOMoveX(isTopSort ? topTarget.position.x : bottomTarget.position.x, moveThroughTime);
            }
            else
            {
                coots.transform.position = isTopSort ? bottomSort.Center : topSort.Center;
                moveTween = coots.transform.DOMoveX(isTopSort ? bottomTarget.position.x : topTarget.position.x, moveThroughTime);
            }
            
            moveTween.OnComplete(() =>
            {
                coots.EnableRigidBody();
                coots.ResetVelocity();
            });
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