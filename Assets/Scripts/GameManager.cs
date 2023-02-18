using System;
using System.Collections.Generic;
using UnityEngine;

namespace SadBrains
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int startingLives;
        [SerializeField] private int toDeliver;
        [SerializeField] private List<CootsType> cootsType;
        
        public static event Action<int> UpdateDelivered;
        public static event Action<int> UpdateLives;
        public static event Action<int> UpdateTime;

        public static GameManager Instance { get; private set; }
        
        private int _delivered;
        private int _lives;
        private int _time;

        public List<CootsType> CootsTypes => cootsType;
        
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
        }
        
        private void OnEnable()
        {
            DeliveryTube.DeliveredBadCoots += OnSubtractLife;
            Pit.DestroyedGoodCoots += OnSubtractLife;
            DeliveryTube.DeliveredGoodCoots += OnDeliveredGoodCoots;
        }

        private void OnDisable()
        {
            DeliveryTube.DeliveredBadCoots -= OnSubtractLife;
            Pit.DestroyedGoodCoots -= OnSubtractLife;
            DeliveryTube.DeliveredGoodCoots -= OnDeliveredGoodCoots;
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
        
    }
}