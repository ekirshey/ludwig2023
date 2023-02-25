using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class CEO : MonoBehaviour
    {
        [SerializeField] private float fadeInSpeed;
        [SerializeField] private SpriteRenderer terminalRenderer;
        [SerializeField] private DialogTerminal terminal;
        [TextArea(3, 10)]
        [SerializeField] private List<string> speech;
        
        [TextArea(3, 10)]
        [SerializeField] private List<string> wonSpeech;

        public IEnumerator RunScript()
        {
            gameObject.SetActive(true);
            terminal.gameObject.SetActive(true);
            yield return terminalRenderer.DOColor(new Color(255, 255, 255, 255), fadeInSpeed).WaitForCompletion();
            terminal.OnNext += OnSpeechFinished;
            var speechFinished = false;
            void OnSpeechFinished()
            {
                speechFinished = true;
            }
            
            foreach (var text in speech)
            {
                speechFinished = false;
                terminal.SetText(text);
                yield return new WaitUntil(() => speechFinished);
                
            }
            terminal.gameObject.SetActive(false);
            terminal.OnNext -= OnSpeechFinished;
            gameObject.SetActive(false);
        }
        
        public IEnumerator WonScript()
        {
            gameObject.SetActive(true);
            terminal.gameObject.SetActive(true);
            yield return terminalRenderer.DOColor(new Color(255, 255, 255, 255), fadeInSpeed).WaitForCompletion();
            terminal.OnNext += OnSpeechFinished;
            var speechFinished = false;
            void OnSpeechFinished()
            {
                speechFinished = true;
            }
            
            foreach (var text in wonSpeech)
            {
                speechFinished = false;
                terminal.SetText(text);
                yield return new WaitUntil(() => speechFinished);
                
            }
            terminal.gameObject.SetActive(false);
            terminal.OnNext -= OnSpeechFinished;
            gameObject.SetActive(false);
        }

    }
}