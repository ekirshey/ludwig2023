using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SadBrains
{
    public abstract class Phase : MonoBehaviour
    {
        public event Action PhaseFinished;
        public static event Action Pause;
        public static event Action Resume;

        [SerializeField] private SpriteRenderer fadeToBlack;
        [SerializeField] protected CatGPT catGpt;

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
            yield return StartCoroutine(Fade(Color.black, 1.0f));
            GameManager.Instance.GameLost();
        }
    }
}