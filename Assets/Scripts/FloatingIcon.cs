using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class FloatingIcon : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private void Start()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(spriteRenderer.DOColor(new Color(0, 0, 0, 0), 2.0f))
                .Join(transform.DOMoveY(transform.position.y + 3, 2.0f))
                .Play().OnComplete(() =>
                {
                    Destroy(gameObject);
                });
        }
    }
}