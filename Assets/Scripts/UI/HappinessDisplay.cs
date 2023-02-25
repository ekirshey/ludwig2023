using UnityEngine;
using UnityEngine.UI;

namespace SadBrains.UI
{
    public class HappinessDisplay : MonoBehaviour
    {
        [SerializeField] private CatGPT catGpt;
        [SerializeField] private Image background;
        [SerializeField] private Image marker;
        [SerializeField] private Image target;

        private RectTransform _displayTransform;
        private float _width;
        
        private void Start()
        {
            target.gameObject.SetActive(false);
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

        public void AddTarget(int level)
        {
            target.gameObject.SetActive(true);
            var xPos = _width * level / catGpt.MaxHappiness;
            target.rectTransform.anchoredPosition = new Vector2(xPos,0);
        }

        public void ClearTarget()
        {
            target.gameObject.SetActive(false);
        }
    }
}