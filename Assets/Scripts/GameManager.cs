using System;
using UnityEngine;

namespace SadBrains
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int startingLives;
        [SerializeField] private int toDeliver;
        
        public static event Action<int> UpdateDelivered;
        public static event Action<int> UpdateLives;
        public static event Action<int> UpdateTime;

        private int _delivered;
        private int _lives;
        private int _time;

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