using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SadBrains
{
    public class AITerminal : MonoBehaviour
    {
        public event Action OnNext;
        
        [SerializeField] private float waitTime;
        [SerializeField] private TMP_Text output;
        [SerializeField] private Button actionButton;

        private bool _textFinished;
        
        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            actionButton.onClick.AddListener(SkipText);
        }

        private void OnDisable()
        {
            actionButton.onClick.RemoveListener(SkipText);
        }

        private void SkipText()
        {
            if (_textFinished)
            {
                OnNext?.Invoke();
                gameObject.SetActive(false);
            }
            else
            {
                _textFinished = true;
            }
        }

        private IEnumerator RevealCharacters(TMP_Text textComponent)
        {
            textComponent.ForceMeshUpdate();

            TMP_TextInfo textInfo = textComponent.textInfo;

            int totalVisibleCharacters = textInfo.characterCount;
            int visibleCount = 0;

            while (visibleCount <= totalVisibleCharacters && !_textFinished)
            {
                textComponent.maxVisibleCharacters = visibleCount; 

                visibleCount += 1;

                yield return new WaitForSeconds(waitTime);
            }

            textComponent.maxVisibleCharacters = totalVisibleCharacters;
            
            _textFinished = true;
            var buttonText = actionButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = "Next";
        }

        public void SetText(String text)
        {
            _textFinished = false;
            var buttonText = actionButton.GetComponentInChildren<TMP_Text>();
            buttonText.text = "Skip";
            gameObject.SetActive(true);
            output.text = text;
            StartCoroutine(RevealCharacters(output));
        }
        
    }
}