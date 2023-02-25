using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class CatGPT : MonoBehaviour
    {
        [SerializeField] private int happinessGain;
        [SerializeField] private int happinessLoss;
        [SerializeField] private int maxHappiness;
        [SerializeField] private int happinessStart;
        [SerializeField] private Animator animator;
        [SerializeField] private DialogTerminal dialogTerminal;
        [SerializeField] private List<OutputObjectType> relevantTypes;
        private static readonly int Boss = Animator.StringToHash("Boss");
        private static readonly int Angrier = Animator.StringToHash("Angrier");
        private static readonly int Angry = Animator.StringToHash("Angry");

        public int Happiness { get; private set; }
        public int MaxHappiness => maxHappiness;
        public bool HappinessLocked { get; set; }

        private void Awake()
        {
            Happiness = happinessStart;
        }

        private void OnEnable()
        {
            Input.DeliveredBadOutputObject += OnDeliveredBadOutputObject;
            Input.DeliveredGoodOutputObject += OnDeliveredGoodOutputObject;
            OutputObject.OutputObjectOffScreen += OnOutputObjectOffScreen;
        }

        private void OnDisable()
        {
            Input.DeliveredBadOutputObject -= OnDeliveredBadOutputObject;
            Input.DeliveredGoodOutputObject -= OnDeliveredGoodOutputObject;
            OutputObject.OutputObjectOffScreen -= OnOutputObjectOffScreen;
        }

        private void OnOutputObjectOffScreen(OutputObjectType obj)
        {
            if (!relevantTypes.Contains(obj)) return;
            
            DeductHappiness(happinessLoss);
        }
        
        private void OnDeliveredGoodOutputObject(OutputObjectType obj)
        {
            if (!relevantTypes.Contains(obj)) return;
            if (HappinessLocked) return;
            Happiness += happinessGain;

            if (Happiness > maxHappiness)
            {
                Happiness = maxHappiness;
            }
            
        }

        private void OnDeliveredBadOutputObject(OutputObjectType obj)
        {
            if (!relevantTypes.Contains(obj)) return;
            DeductHappiness(happinessLoss);
        }
        
        
        public void DeductHappiness(int deduction)
        {
            if (HappinessLocked) return;
            Happiness -= deduction;
            if (Happiness < 0)
            {
                Happiness = 0;
            }
        }

        public IEnumerator Speak(List<string> dialog)
        {
            dialogTerminal.gameObject.SetActive(true);
            foreach (var text in dialog)
            {
                var speechFinished = false;

                void OnSpeechFinished()
                {
                    speechFinished = true;
                }

                dialogTerminal.OnNext += OnSpeechFinished;
                dialogTerminal.SetText(text);
                yield return new WaitUntil(() => speechFinished);
                dialogTerminal.OnNext -= OnSpeechFinished;
            }

            dialogTerminal.gameObject.SetActive(false);
        }

        public IEnumerator MoveToTarget(Vector3 destination, float moveDuration)
        {
            yield return transform.DOMove(destination, moveDuration).WaitForCompletion();
        }

        public void SetAnger(int value)
        {
            switch (value)
            {
                case 0:
                    animator.SetTrigger(Angry);
                    break;
                case 1:
                    animator.SetTrigger(Angrier);
                    break;
                case 2:
                    animator.SetTrigger(Boss);
                    break;
            }
        }

    }
}