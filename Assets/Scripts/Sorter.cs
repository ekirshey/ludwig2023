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
        
        private OutputObjectType _allowedObjectType;

        protected override void OnEnable()
        {
            base.OnEnable();
            typeSelect.OutputObjectTypeChanged += OnOutputObjectTypeChanged;
            topSort.OutputObjectEntered += MoveOutputObject;
            bottomSort.OutputObjectEntered += MoveOutputObject;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            typeSelect.OutputObjectTypeChanged -= OnOutputObjectTypeChanged;
            topSort.OutputObjectEntered -= MoveOutputObject;
            bottomSort.OutputObjectEntered -= MoveOutputObject;
        }
        
        private void OnOutputObjectTypeChanged(OutputObjectType type)
        {
            _allowedObjectType = type;
        }

        private void MoveOutputObject(OutputObject outputObject)
        {
            outputObject.ResetVelocity();

            Vector3 targetPosition;
            if (outputObject.OutputObjectType == _allowedObjectType)
            {
                targetPosition = isTopSort ? topSort.Center : bottomSort.Center;
                targetPosition.x = isTopSort ? topTarget.position.x : bottomTarget.position.x;
            }
            else
            {
                targetPosition = isTopSort ? bottomSort.Center : topSort.Center;
                targetPosition.x = isTopSort ? bottomTarget.position.x : topTarget.position.x;
            }

            outputObject.transform.position = targetPosition;
        }

        
        public override void DisableCollision()
        {
            base.DisableCollision();
            var color = spriteRenderer.color;
            color.a *= 0.5f;
            spriteRenderer.color = color;
        }

        public override void EnableCollision()
        {
            base.EnableCollision();
            var color = spriteRenderer.color;
            color.a *= 2f;
            spriteRenderer.color = color;
        }
        
        public override void Destroy()
        {
            Destroy(gameObject);
        }
    }
}