using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SadBrains
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private List<CootsType> cootsTypes;
        [SerializeField] private List<Vector3> leftIOLocations;
        [SerializeField] private List<Vector3> rightIOLocations;
        [SerializeField] private List<Rect> disallowedRegions;
        
        public static GameManager Instance { get; private set; }
        
        private int _delivered;
        private int _lives;
        private int _time;

        public List<CootsType> CootsTypes => cootsTypes;
        public List<Vector3> LeftIOLocations => leftIOLocations;
        public List<Vector3> RightIOLocations => rightIOLocations;
        public List<Tuple<CootsOutput, CootsInput>> IoPair { get; private set; }
        
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

            IoPair = new List<Tuple<CootsOutput, CootsInput>>();
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
            _gamePhases[_currentPhaseIdx].PhaseFinished += OnPhaseFinished;
        }
        
        public void AddIO(CootsOutput output, CootsInput input)
        {
            IoPair.Add(new Tuple<CootsOutput, CootsInput>(output, input));
        }
        
        public void RemoveIO(Tuple<CootsOutput, CootsInput> io)
        {
            IoPair.Remove(io);
        }
        
        public bool InDisallowedRegion(Vector2 center, Vector2 size)
        {
            var objectRect = new Rect(center - size/2, size);
            foreach (var rect in disallowedRegions)
            {
                if (rect.Overlaps(objectRect))
                {
                    return true;
                }
            }

            return false;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            foreach (var io in leftIOLocations)
            {
                var center = io;
                center.x += 2f;
                center.y -= 2f;
                
                Gizmos.DrawWireCube(center, new Vector3(4,4,0));
            }
            
            foreach (var io in rightIOLocations)
            {
                var center = io;
                center.x += 2f;
                center.y -= 2f;
                
                Gizmos.DrawWireCube(center, new Vector3(4,4,0));
            }

            Gizmos.color = Color.red;
            foreach (var rect in disallowedRegions)
            {
                Gizmos.DrawWireCube(rect.center, new Vector3(rect.width, rect.height, 0));
            }
        }


    }
}