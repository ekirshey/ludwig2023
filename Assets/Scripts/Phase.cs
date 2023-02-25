using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public abstract class Phase : MonoBehaviour
    {
        public static event Action DestroyDevices;
        public event Action PhaseFinished;
        public static event Action Pause;
        public static event Action Resume;

        [SerializeField] private SpriteRenderer fadeToBlack;
        [SerializeField] protected CatGPT catGpt;
        [SerializeField] private Vector3 startPosition;
        [SerializeField] private Vector3 endPosition;
        [SerializeField] private float duration;
        [TextArea(3,10)]
        [SerializeField] public List<string> gameLostMessages;
        
        protected bool Active;
        protected List<GameManager.IOPair> AvailableIOPairs;
        protected List<Vector3> AvailableLeftIOSpawns;
        protected List<Vector3> AvailableRightIOSpawns;

        public virtual void SetActive()
        {
            Active = true;
            var cootsTypes = GameManager.Instance.IOPairs;

            AvailableIOPairs = new List<GameManager.IOPair>();
            AvailableIOPairs.AddRange(cootsTypes);
            
            AvailableLeftIOSpawns = new List<Vector3>();
            AvailableLeftIOSpawns.AddRange(GameManager.Instance.LeftIOLocations);
            
            AvailableRightIOSpawns = new List<Vector3>();
            AvailableRightIOSpawns.AddRange(GameManager.Instance.RightIOLocations);
        }
        
        protected void Finish()
        {
            Active = false;
            PhaseFinished?.Invoke();
        }

        protected void PauseAll()
        {
            Pause?.Invoke();
        }
        
        protected void EnableAll()
        {
            Resume?.Invoke();
        }

        protected void CleanupDevices()
        {
            DestroyDevices?.Invoke();
        }

        protected IEnumerator Fade(Color targetColor, float duration)
        {
            yield return fadeToBlack.DOColor(targetColor, duration).WaitForCompletion();
        }
        
        protected IEnumerator GameWon()
        { 
            Active = false;
            yield return StartCoroutine(Fade(Color.black, 1.0f));
            GameManager.Instance.GameWon();
        }

        protected IEnumerator GameLost()
        {
            Active = false;
            catGpt.transform.position = startPosition;
            yield return StartCoroutine(catGpt.MoveToTarget(endPosition, duration));
            yield return StartCoroutine(catGpt.Speak(gameLostMessages));
            
            yield return StartCoroutine(Fade(Color.black, 1.0f));
            GameManager.Instance.GameLost();
        }
    }
}