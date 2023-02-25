﻿using System;
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
        [SerializeField] private DialogTerminal dialogTerminal;
        [SerializeField] private List<OutputObjectType> relevantTypes;

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

    }
}