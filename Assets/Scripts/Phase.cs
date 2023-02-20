using System;
using System.Collections.Generic;
using UnityEngine;

namespace SadBrains
{
    public abstract class Phase : MonoBehaviour
    {
        public event Action PhaseFinished;
        
        protected List<CootsType> AvailableCootsTypes;
        protected List<Vector3> AvailableLeftIOSpawns;
        protected List<Vector3> AvailableRightIOSpawns;
        protected Dictionary<CootsType, int> DeliveredCoots;
        
        protected bool Active { get; set; }
        
        public virtual void SetActive()
        {
            Active = true;

            var cootsTypes = GameManager.Instance.CootsTypes;
            DeliveredCoots = new Dictionary<CootsType, int>();
            foreach (var type in cootsTypes)
            {
                DeliveredCoots.Add(type, 0);
            }
            
            AvailableCootsTypes = new List<CootsType>();
            AvailableCootsTypes.AddRange(cootsTypes);
            
            AvailableLeftIOSpawns = new List<Vector3>();
            AvailableLeftIOSpawns.AddRange(GameManager.Instance.LeftIOLocations);
            
            AvailableRightIOSpawns = new List<Vector3>();
            AvailableRightIOSpawns.AddRange(GameManager.Instance.RightIOLocations);
        }

        public abstract void OnDeliveredBadCoots(CootsType received, CootsType expected);
        public abstract void OnDeliveredGoodCoots(CootsType type);

    }
}