using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private int numFishToDeliver;

        [Tooltip("UI")]
        [SerializeField] private GameObject bossUI;
        [SerializeField] private Timer timer;
        [SerializeField] private ItemDisplay topSorters;
        [SerializeField] private ItemDisplay bottomSorters;
        [SerializeField] private Placeable topFishSorter;
        [SerializeField] private Placeable bottomFishSorter;
        [SerializeField] private HappinessDisplay happinessDisplay;
        
        [Tooltip("CatGPT")]
        [SerializeField] private Vector3 catGptStartPosition;
        [SerializeField] private Vector3 catGptPosition;
        [SerializeField] private float catGptSpeed;
        [SerializeField] private List<string> preDestructionText;
        [SerializeField] private List<string> speech;
        [SerializeField] private CEO ceo;
        
        [Tooltip("camera")]
        [SerializeField] private float fadeTime;
        [SerializeField] private ScreenShakeController shakeController;
        [SerializeField] private ScreenShakeController.ScreenShakeParameters destructionShake;

        private Dictionary<OutputObjectType, int> _fishTracker;

        private void Awake()
        {
            bossUI.gameObject.SetActive(false);
            _fishTracker = new Dictionary<OutputObjectType, int>
            {
                {OutputObjectType.PurpleFish, numFishToDeliver},
                {OutputObjectType.YellowFish, numFishToDeliver},
                {OutputObjectType.RedFish, numFishToDeliver}
            };
        }

        private void OnEnable()
        {
            Input.DeliveredGoodOutputObject += OnFishDelivered;
        }

        private void OnDisable()
        {
            Input.DeliveredGoodOutputObject -= OnFishDelivered;
        }

        private void OnFishDelivered(OutputObjectType obj)
        {
            if (_fishTracker.ContainsKey(obj))
            {
                _fishTracker[obj]--;

                if (_fishTracker[obj] <= 0)
                {
                    _fishTracker.Remove(obj);
                }
            }

            if (_fishTracker.Count == 0)
            {
                StartCoroutine(GameWon());
            }
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
            yield return StartCoroutine(Fade(Color.black, fadeTime));
            happinessDisplay.gameObject.SetActive(false);
            catGpt.transform.position = catGptStartPosition;
            bossLevel.AddDebris();
            yield return StartCoroutine(Fade(new Color(0,0,0,0), fadeTime));
            
            yield return StartCoroutine( ceo.RunScript());
            
            yield return StartCoroutine(GameManager.Instance.DeleteIO(1.0f));
            yield return StartCoroutine(bossLevel.InitializeIO());
            
            EnableAll();

            bossUI.gameObject.SetActive(true);
            timer.TimerFinished += OnTimerFinished;
            StartCoroutine(timer.RunTimer(countdownTime));
        }

        private void OnTimerFinished()
        {
            timer.TimerFinished -= OnTimerFinished;

            StartCoroutine(_fishTracker.Count == 0 ? GameWon() : GameLost());
        }

        public override void SetActive()
        {
            base.SetActive();
            StartCoroutine(BossEvent());
        }
        
    }
}