using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SadBrains
{
    public class GameManager : MonoBehaviour
    {
        [Serializable]
        public class IOPair
        {
            public Output output;
            public Input input;
        }

        [SerializeField] private List<IOPair> ioPairs;
        [SerializeField] private List<Vector3> leftIOLocations;
        [SerializeField] private List<Vector3> rightIOLocations;
        [SerializeField] private List<Rect> disallowedRegions;
        [SerializeField] private List<Phase> gamePhases;

        public static GameManager Instance { get; private set; }
        
        private int _delivered;
        private int _lives;
        private int _time;

        public List<IOPair> IOPairs => ioPairs;
        public List<Vector3> LeftIOLocations => leftIOLocations;
        public List<Vector3> RightIOLocations => rightIOLocations;
        public List<Input> Inputs { get; private set; }
        public List<Output> Outputs { get; private set; }
        
        private int _currentPhaseIdx;

        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            Inputs = new List<Input>();
            Outputs = new List<Output>();
        }

        private void Start()
        {
            gamePhases[_currentPhaseIdx].SetActive();
            gamePhases[_currentPhaseIdx].PhaseFinished += OnPhaseFinished;
        }

        private void OnPhaseFinished()
        {
            gamePhases[_currentPhaseIdx].PhaseFinished -= OnPhaseFinished;
            
            _currentPhaseIdx++;

            gamePhases[_currentPhaseIdx].SetActive();
            gamePhases[_currentPhaseIdx].PhaseFinished += OnPhaseFinished;
        }
        
        public void AddIO(Output output, Input input)
        {
            Outputs.Add(output);
            Inputs.Add(input);
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
        
        public IEnumerator DeleteIO(float retractSpeed)
        {
            var retract = DOTween.Sequence();

            foreach (var output in Outputs)
            {
                var outputTransform = output.transform;
                retract.Join(outputTransform.DOMoveX(outputTransform.position.x - 4, retractSpeed));
            }
            
            foreach (var input in Inputs)
            {
                var inputTransform = input.transform;
                retract.Join(inputTransform.DOMoveX(inputTransform.position.x + 4, retractSpeed));
            }

            yield return retract.Play().WaitForCompletion();
            
            foreach (var output in Outputs)
            {
                Destroy(output.gameObject);
            }
                
            foreach (var input in Inputs)
            {
                Destroy(input.gameObject);
            }
        }

        public void GameLost()
        {
            SceneManager.LoadScene("GameLost");
        }

        public void GameWon()
        {
            SceneManager.LoadScene("GameWon");
        }
    }
}