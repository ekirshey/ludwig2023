using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SadBrains.UI
{
    public class BlinkableUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color flashColor;
        [SerializeField] private float time;

        private Tween _blinkTween;

        private void Start()
        {
            image.color = defaultColor;
        }
        
        public void Blink()
        {
            _blinkTween = image.DOColor(flashColor,  time).SetEase(Ease.Flash, 15);
        }

        public void EndBlink()
        {
            _blinkTween.Kill();
        }
    }
}