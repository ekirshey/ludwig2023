
using System.Collections;
using System.Collections.Generic;
using SadBrains.UI;
using UnityEngine;

namespace SadBrains
{
    public class BossPhase : Phase
    {
        
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
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip bossMusic;
        [SerializeField] private AudioClip wonMusic;
        [SerializeField] private Color bgColor;
        [SerializeField] private float fadeTime;
        [SerializeField] private ScreenShakeController shakeController;
        [SerializeField] private ScreenShakeController.ScreenShakeParameters destructionShake;

        private Dictionary<OutputObjectType, int> _fishTracker;
        private bool _gameOver;

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

        private IEnumerator Win()
        {
            timer.TimerFinished -= OnTimerFinished;
            timer.ExitTimer();
            bossUI.gameObject.SetActive(false);
            source.clip = wonMusic;
            source.Play();
            yield return StartCoroutine(ceo.WonScript());
            yield return StartCoroutine(GameWon());
        }
        
        private void OnFishDelivered(OutputObjectType obj)
        {
            if (_gameOver) return;
            if (_fishTracker.ContainsKey(obj))
            {
                _fishTracker[obj]--;

                if (_fishTracker[obj] <= 0)
                {
                    _fishTracker.Remove(obj);
                }
            }

            if (_fishTracker.Count != 0) return;
            _gameOver = true;
            StartCoroutine(Win());
        }

        private IEnumerator CatGptIntro()
        {
            catGpt.SetAnger(2);
            catGpt.transform.position = catGptStartPosition;
            yield return StartCoroutine(catGpt.MoveToTarget(catGptPosition, catGptSpeed));
            yield return StartCoroutine(catGpt.Speak(speech));
        }
        
        private IEnumerator BossEvent()
        {
            PauseAll();
            bossLevel.gameObject.SetActive(true);
            CleanupDevices();
            topSorters.ChangeType(topFishSorter);
            bottomSorters.ChangeType(bottomFishSorter);
            yield return StartCoroutine(CatGptIntro());
            shakeController.Shake(destructionShake);
            yield return StartCoroutine(Fade(Color.black, fadeTime));
            happinessDisplay.gameObject.SetActive(false);
            catGpt.transform.position = catGptStartPosition;
            bossLevel.AddDebris();
            source.clip = bossMusic;
            source.Play();
            Camera.main.backgroundColor = bgColor;
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