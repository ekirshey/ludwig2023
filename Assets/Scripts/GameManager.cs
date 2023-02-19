using System;
using System.Collections;
using System.Collections.Generic;
using SadBrains.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SadBrains
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int startingLives;
        [SerializeField] private List<CootsType> cootsTypes;
        [SerializeField] private List<Vector3> leftIOLocations;
        [SerializeField] private List<Vector3> rightIOLocations;
        [SerializeField] private CootsOutput outputPrefab;
        [SerializeField] private CootsInput inputPrefab;
        [SerializeField] private float ioSpawnRate;
        
        public static event Action<int> UpdateDelivered;
        public static event Action<int> UpdateLives;
        
        public static GameManager Instance { get; private set; }
        
        private int _delivered;
        private int _lives;
        private int _time;

        public List<CootsType> CootsTypes => cootsTypes;

        private List<CootsType> _availableCootsTypes;
        private List<Vector3> _availableLeftIOSpawns;
        private List<Vector3> _availableRightIOSpawns;
        
        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(gameObject);
                return;
                
            }

            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _lives = startingLives;
            UpdateLives?.Invoke(_lives);

            _availableCootsTypes = new List<CootsType>();
            _availableCootsTypes.AddRange(cootsTypes);
            
            _availableLeftIOSpawns = new List<Vector3>();
            _availableLeftIOSpawns.AddRange(leftIOLocations);
            
            _availableRightIOSpawns = new List<Vector3>();
            _availableRightIOSpawns.AddRange(rightIOLocations);

            StartCoroutine(IOTimer());

        }

        private void CreateIO()
        {
            var side = Random.Range(0, 2);

            Vector3 outputLoc;
            Vector3 inputLoc;
            side = 0;
            if (side == 0)
            {
                outputLoc = _availableLeftIOSpawns.PopRandomElement();
                inputLoc = _availableRightIOSpawns.PopRandomElement();
            }
            else
            {
                outputLoc = _availableRightIOSpawns.PopRandomElement();
                inputLoc = _availableLeftIOSpawns.PopRandomElement();
            }

            var cootsType = _availableCootsTypes.PopRandomElement();
            var output = Instantiate(outputPrefab);
            output.transform.position = outputLoc;
            output.SetCootsType(cootsType);
            
            var input = Instantiate(inputPrefab);
            input.transform.position = inputLoc;
            input.SetCootsType(cootsType);
        }
        
        private IEnumerator IOTimer()
        {
            while (_availableCootsTypes.Count > 0)
            {
                CreateIO();
                yield return new WaitForSeconds(ioSpawnRate);
            }
        }

        private void OnEnable()
        {
            CootsInput.DeliveredBadCoots += OnSubtractLife;
            Pit.DestroyedGoodCoots += OnSubtractLife;
            CootsInput.DeliveredGoodCoots += OnDeliveredGoodCoots;
        }

        private void OnDisable()
        {
            CootsInput.DeliveredBadCoots -= OnSubtractLife;
            Pit.DestroyedGoodCoots -= OnSubtractLife;
            CootsInput.DeliveredGoodCoots -= OnDeliveredGoodCoots;
        }

        private void OnSubtractLife()
        {
            _lives -= 1;
            UpdateLives?.Invoke(_lives);
        }

        private void OnDeliveredGoodCoots()
        {
            _delivered += 1;
            UpdateDelivered?.Invoke(_delivered);
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