using System;
using System.Collections.Generic;
using UnityEngine;

namespace SadBrains
{
    public abstract class Phase : MonoBehaviour
    {
        [SerializeField] protected CatGPT catGpt;
        
        public event Action PhaseFinished;
        public static event Action Pause;
        public static event Action Resume;

        protected List<GameManager.IOPair> AvailableIOPairs;
        protected List<Vector3> AvailableLeftIOSpawns;
        protected List<Vector3> AvailableRightIOSpawns;
        
        protected bool Active { get; set; }

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
    }
}