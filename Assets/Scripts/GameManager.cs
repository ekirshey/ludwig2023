﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SadBrains
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<CootsType> cootsTypes;
        [SerializeField] private List<Vector3> leftIOLocations;
        [SerializeField] private List<Vector3> rightIOLocations;
        
        public static GameManager Instance { get; private set; }
        
        private int _delivered;
        private int _lives;
        private int _time;

        public List<CootsType> CootsTypes => cootsTypes;
        public List<Vector3> LeftIOLocations => leftIOLocations;
        public List<Vector3> RightIOLocations => rightIOLocations;
        
        private List<Phase> _gamePhases;
        private int _currentPhaseIdx;

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            DontDestroyOnLoad(gameObject);

            _gamePhases = gameObject.GetComponentsInChildren<Phase>().ToList();
        }

        private void Start()
        {
            _gamePhases[_currentPhaseIdx].SetActive();
            _gamePhases[_currentPhaseIdx].PhaseFinished += OnPhaseFinished;
        }

        private void OnPhaseFinished()
        {
            _gamePhases[_currentPhaseIdx].PhaseFinished -= OnPhaseFinished;
            _currentPhaseIdx++;
            _gamePhases[_currentPhaseIdx].SetActive();
        }

        private void OnEnable()
        {
            CootsInput.DeliveredBadCoots += OnDeliveredBadCoots;
            CootsInput.DeliveredGoodCoots += OnDeliveredGoodCoots;
        }

        private void OnDisable()
        {
            CootsInput.DeliveredBadCoots -= OnDeliveredBadCoots;
            CootsInput.DeliveredGoodCoots -= OnDeliveredGoodCoots;
        }

        private void OnDeliveredBadCoots(CootsType received, CootsType expected)
        {
            _gamePhases[_currentPhaseIdx].OnDeliveredBadCoots(received, expected);
        }

        private void OnDeliveredGoodCoots(CootsType type)
        {
            _gamePhases[_currentPhaseIdx].OnDeliveredGoodCoots(type);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (var io in leftIOLocations)
            {
                var center = io;
                center.x += 2f;
                center.y -= 1.5f;
                
                Gizmos.DrawWireCube(center, new Vector3(4,3,0));
            }
            
            foreach (var io in rightIOLocations)
            {
                var center = io;
                center.x += 2f;
                center.y -= 1.5f;
                
                Gizmos.DrawWireCube(center, new Vector3(4,3,0));
            }
        }
    }
}