using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public class CatGPT : MonoBehaviour
    {
        public event Action GameLost;
        
        [SerializeField] private int happinessGain;
        [SerializeField] private int happinessLoss;
        [SerializeField] private int maxHappiness;
        [SerializeField] private int happinessStart;
        [SerializeField] private AITerminal aiTerminal;

        public int Happiness { get; private set; }
        public int MaxHappiness => maxHappiness;
        public delegate void HappinessAlert();
        public delegate void SpeechFinished();

        private List<Tuple<HappinessAlert, int>> _happinessAlerts;
        
        private void Awake()
        {
            Happiness = happinessStart;
            _happinessAlerts = new List<Tuple<HappinessAlert, int>>();
        }

        private void OnEnable()
        {
            CootsInput.DeliveredBadCoots += OnDeliveredBadCoots;
            CootsInput.DeliveredGoodCoots += OnDeliveredGoodCoots;
            Coots.CootsDestroyed += OnCootsDestroyed;
        }

        private void OnDisable()
        {
            CootsInput.DeliveredBadCoots -= OnDeliveredBadCoots;
            CootsInput.DeliveredGoodCoots -= OnDeliveredGoodCoots;
            Coots.CootsDestroyed -= OnCootsDestroyed;
        }

        private void OnCootsDestroyed()
        {
            DeductHappiness(happinessLoss);
        }
        
        private void OnDeliveredGoodCoots(CootsType obj)
        {
            Happiness += happinessGain;
            if (Happiness > maxHappiness)
            {
                Happiness = maxHappiness;
            }

            for (var i = _happinessAlerts.Count - 1; i >= 0; i--)
            {
                var alert = _happinessAlerts[i];
                if (Happiness >= alert.Item2)
                {
                    alert.Item1.Invoke();
                    _happinessAlerts.RemoveAt(i);
                }
            }
        }

        private void OnDeliveredBadCoots(CootsType arg1, CootsType arg2)
        {
            DeductHappiness(happinessLoss);
        }

        public void AddHappinessAlert(HappinessAlert alert, int value)
        {
            _happinessAlerts.Add(new Tuple<HappinessAlert, int>(alert, value));
        }
        
        public void DeductHappiness(int deduction)
        {
            Happiness -= deduction;
            if (Happiness <= 0)
            {
                Happiness = 0;
                GameLost?.Invoke();
            }
        }

        public IEnumerator Speak(string text)
        {
            var speechFinished = false;
            void OnSpeechFinished()
            {
                speechFinished = true;
            }

            aiTerminal.OnNext += OnSpeechFinished;
            aiTerminal.SetText(text);
            yield return new WaitUntil(() => speechFinished);
            aiTerminal.OnNext -= OnSpeechFinished;
        }

        public IEnumerator MoveToTarget(Vector3 destination, float moveDuration)
        {
            yield return transform.DOMove(destination, moveDuration).WaitForCompletion();
        }
        

    }
}