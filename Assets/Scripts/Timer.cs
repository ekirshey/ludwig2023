using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace SadBrains
{
    public class Timer : MonoBehaviour
    {
        public event Action TimerFinished;
        
        [SerializeField] private TMP_Text timerText;

        private int _time;

        private void Awake()
        {
            timerText.gameObject.SetActive(false);    
        }
        
        private IEnumerator RunTimer()
        {
            var count = 0;
            while (count < _time)
            {
                count++;
                timerText.text = (_time - count).ToString();
                yield return new WaitForSeconds(1.0f);
            }
            timerText.gameObject.SetActive(false);
            TimerFinished?.Invoke();
        }

        public void SetTimer(int time)
        {
            timerText.gameObject.SetActive(true);
            _time = time;
        }
    }
}