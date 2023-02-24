using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SadBrains
{
    public class BossPhase : Phase
    {
        public static event Action DestroyDevices;
        public static event Action SortFish;
        
        [Tooltip("Mechanics")]
        [SerializeField] private BossLevel bossLevel;

        [Tooltip("UI")]
        [SerializeField] private GameObject bossUI;
        [SerializeField] private Timer timer;
        
        [Tooltip("CatGPT")]
        [SerializeField] private Vector3 catGptStartPosition;
        [SerializeField] private Vector3 catGptPosition;
        [SerializeField] private float catGptSpeed;
        [SerializeField] private List<string> preDestructionText;
        [SerializeField] private List<string> speech;
        [SerializeField] private ScreenShakeController screenShake;
        [SerializeField] private ScreenShakeController.ScreenShakeParameters shakeParams;
        
        private IEnumerator CatGptIntro()
        {
            catGpt.transform.position = catGptStartPosition;
            yield return StartCoroutine(catGpt.MoveToTarget(catGptPosition, catGptSpeed));
            foreach (var text in speech)
            {
                yield return StartCoroutine(catGpt.Speak(text));
            }
            yield return StartCoroutine(catGpt.MoveToTarget(catGptStartPosition, catGptSpeed));
        }
        
        private IEnumerator BossEvent()
        {
            screenShake.Shake(shakeParams);
            PauseAll();
            bossLevel.gameObject.SetActive(true);
            DestroyDevices?.Invoke();
            SortFish?.Invoke();
            yield return StartCoroutine(CatGptIntro());
            yield return StartCoroutine(bossLevel.DropDebris());
            yield return StartCoroutine(GameManager.Instance.DeleteIO(1.0f));
            yield return StartCoroutine(bossLevel.InitializeIO());
            EnableAll();
            yield return null;
        }

        public override void SetActive()
        {
            base.SetActive();
            StartCoroutine(BossEvent());
        }
        
    }
}