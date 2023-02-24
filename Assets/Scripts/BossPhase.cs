using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SadBrains.UI;
using UnityEngine;

namespace SadBrains
{
    public class BossPhase : Phase
    {
        public static event Action DestroyDevices;
        
        [Tooltip("Mechanics")]
        [SerializeField] private BossLevel bossLevel;
        [SerializeField] private int countdownTime;

        [Tooltip("UI")]
        [SerializeField] private GameObject bossUI;
        [SerializeField] private Timer timer;
        [SerializeField] private ItemDisplay topSorters;
        [SerializeField] private ItemDisplay bottomSorters;
        [SerializeField] private Placeable topFishSorter;
        [SerializeField] private Placeable bottomFishSorter;
        
        [Tooltip("CatGPT")]
        [SerializeField] private Vector3 catGptStartPosition;
        [SerializeField] private Vector3 catGptPosition;
        [SerializeField] private float catGptSpeed;
        [SerializeField] private List<string> preDestructionText;
        [SerializeField] private List<string> speech;
        [SerializeField] private CEO ceo;
        
        [Tooltip("camera")]
        [SerializeField] private SpriteRenderer fadeToBlack;
        [SerializeField] private float fadeTime;
        [SerializeField] private ScreenShakeController shakeController;
        [SerializeField] private ScreenShakeController.ScreenShakeParameters destructionShake;
        
        private void Awake()
        {
            bossUI.gameObject.SetActive(false);
        }

        private IEnumerator CatGptIntro()
        {
            catGpt.transform.position = catGptStartPosition;
            yield return StartCoroutine(catGpt.MoveToTarget(catGptPosition, catGptSpeed));
            foreach (var text in speech)
            {
                yield return StartCoroutine(catGpt.Speak(text));
            }
        }
        
        private IEnumerator BossEvent()
        {
            PauseAll();
            bossLevel.gameObject.SetActive(true);
            DestroyDevices?.Invoke();
            topSorters.ChangeType(topFishSorter);
            bottomSorters.ChangeType(bottomFishSorter);
            yield return StartCoroutine(CatGptIntro());
            shakeController.Shake(destructionShake);
            yield return fadeToBlack.DOColor(Color.black, fadeTime).WaitForCompletion();
            catGpt.transform.position = catGptStartPosition;
            bossLevel.AddDebris();
            yield return fadeToBlack.DOColor(new Color(0,0,0,0), fadeTime).WaitForCompletion();
            
            yield return StartCoroutine( ceo.RunScript());
            
            yield return StartCoroutine(GameManager.Instance.DeleteIO(1.0f));
            yield return StartCoroutine(bossLevel.InitializeIO());
            
            EnableAll();

            timer.TimerFinished += OnTimerFinished;
            StartCoroutine(timer.RunTimer(countdownTime));
            bossUI.gameObject.SetActive(true);
        }

        private void OnTimerFinished()
        {
            timer.TimerFinished -= OnTimerFinished;
            
            // Check for win condition
        }

        public override void SetActive()
        {
            base.SetActive();
            StartCoroutine(BossEvent());
        }
        
    }
}