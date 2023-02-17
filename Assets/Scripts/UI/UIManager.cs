using TMPro;
using UnityEngine;

namespace SadBrains.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text delivered;
        [SerializeField] private TMP_Text lives;
        [SerializeField] private TMP_Text timer;

        private void OnEnable()
        {
            GameManager.UpdateDelivered += OnUpdateDelivered;
            GameManager.UpdateLives += OnUpdateLives;
        }

        private void OnDisable()
        {
            GameManager.UpdateDelivered -= OnUpdateDelivered;
            GameManager.UpdateLives -= OnUpdateLives;
        }

        private void Start()
        {
            delivered.text = "Delivered: " + 0;
        }
        
        private void OnUpdateDelivered(int updatedDelivered)
        {
            delivered.text = "Delivered: " + updatedDelivered;
        }
        
        private void OnUpdateLives(int updatedLives)
        {
            lives.text = "Lives: " + updatedLives;
        }
    }
}