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
        private bool _exit;

        private void Awake()
        {
            timerText.gameObject.SetActive(false);    
        }
        
        public IEnumerator RunTimer(int time)
        {
            timerText.gameObject.SetActive(true);
            _time = time;
            var count = 0;
            while (count < _time && !_exit)
            {
                count++;
                timerText.text = (_time - count).ToString();
                yield return new WaitForSeconds(1.0f);
            }

            _exit = false;
            timerText.gameObject.SetActive(false);
            TimerFinished?.Invoke();
        }

        public void ExitTimer()
        {
            _exit = true;
        }
        
    }
}