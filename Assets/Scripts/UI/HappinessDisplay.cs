using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SadBrains.UI
{
    public class HappinessDisplay : MonoBehaviour
    {
        [SerializeField] private CatGPT catGpt;
        [SerializeField] private Image background;
        [SerializeField] private Image marker;

        private RectTransform _displayTransform;
        private float _width;
        
        private void Start()
        {
            _displayTransform = (RectTransform) background.transform;
            _width = _displayTransform.sizeDelta.x;

            var xPos = _width * catGpt.Happiness / catGpt.MaxHappiness;
            marker.rectTransform.anchoredPosition = new Vector2(xPos,0);
        }
        
        private void Update()
        {
            var xPos = _width * catGpt.Happiness / catGpt.MaxHappiness;
            marker.rectTransform.anchoredPosition = new Vector2(xPos,0);
        }
    }
}