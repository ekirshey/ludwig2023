using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SadBrains
{
    public class CootsOutput : MonoBehaviour
    {
        [SerializeField] private Coots cootsPrefabs;
        [SerializeField] private float spawnRate;
        [SerializeField] private Transform spawnLocation;
        [SerializeField] private int defaultWaitTime;
        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private Image typeImage;
        [SerializeField] private Button skipButton;

        private CootsType _cootsToSpawn;
        private bool _skip;
        
        private void Start()
        {
            countdownText.text = defaultWaitTime.ToString();
            StartCoroutine(Wait());
        }

        private void OnEnable()
        {
            skipButton.onClick.AddListener(OnSkip);
        }

        private void OnDisable()
        {
            skipButton.onClick.RemoveListener(OnSkip);
        }
        
        private void OnSkip()
        {
            _skip = true;
        }

        private IEnumerator Wait()
        {
            var count = 0;
            while (count < defaultWaitTime && !_skip)
            {
                count++;
                countdownText.text = (defaultWaitTime - count).ToString();
                yield return new WaitForSeconds(1.0f);
            }
            countdownText.gameObject.SetActive(false);
            StartCoroutine(Spawn());
        }
        
        private IEnumerator Spawn()
        {
            while (true)
            {
                var newCoots = Instantiate(cootsPrefabs, transform);
                newCoots.transform.position = spawnLocation.position;
                newCoots.SetType(_cootsToSpawn);
                
                yield return new WaitForSeconds(spawnRate);
            }
        }

        public void SetCootsType(CootsType cootsType)
        {
            _cootsToSpawn = cootsType;
            typeImage.sprite = cootsType.sprite;
        }
    }
}