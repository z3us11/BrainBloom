using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Eduzo.Games.FactoryRush
{
    public class FactoryRushGameplay : MonoBehaviour
    {
        [SerializeField] FactoryRushGameManager gameManager;
        [Header("UI")]
        [SerializeField] GameObject[] hearts;
        [SerializeField] TMP_Text timerTxt;
        [SerializeField] TMP_Text midText;
        float timer;
        [Header("Conveyer Boxes")]
        [SerializeField] float boxMoveSpeed;
        [SerializeField] RectTransform leftBox;
        [SerializeField] float leftBoxStartPos;
        [SerializeField] float leftBoxEndPos;
        [SerializeField] RectTransform rightBox;
        [SerializeField] float rightBoxStartPos;
        [SerializeField] float rightBoxEndPos;
        [Header("Main Belt")]
        [SerializeField] float mainBoxStartPos;
        [SerializeField] float mainBoxMidPos;
        [SerializeField] float mainBoxEndPos;
        [SerializeField] float mainBoxSpeed;
        [SerializeField] RectTransform mainBox;
        [SerializeField] GameObject trafficLightYellow;
        [SerializeField] GameObject trafficLightGreen;

        bool gameStarted;

        private void Awake()
        {
            gameManager.OnGameStart += HandleGameStart;
        }

        private void OnDestroy()
        {
            gameManager.OnGameStart -= HandleGameStart;
        }

        private void Start()
        {
            leftBox.anchoredPosition = new Vector2(leftBoxStartPos, leftBox.anchoredPosition.y);
            rightBox.anchoredPosition = new Vector2(rightBoxStartPos, rightBox.anchoredPosition.y);
            mainBox.anchoredPosition = new Vector2(mainBoxStartPos, mainBox.anchoredPosition.y);
        }

        private void HandleGameStart()
        {
            timer = 120;
            timerTxt.text = FormatTime(timer);

            StartCoroutine(StartMidTimer());
        }

        IEnumerator StartMidTimer()
        {
            midText.text = "3";
            yield return new WaitForSeconds(1f);
            midText.text = "2";
            yield return new WaitForSeconds(1f);
            midText.text = "1";
            yield return new WaitForSeconds(1f);
            midText.text = "Start!";

            gameStarted = true;
            MoveConveyerBeltBoxes();

            yield return new WaitForSeconds(2f);
        }

        void MoveConveyerBeltBoxes()
        {
            leftBox.DOAnchorPos(new Vector2(leftBoxEndPos, leftBox.anchoredPosition.y), boxMoveSpeed);
            rightBox.DOAnchorPos(new Vector2(rightBoxEndPos, rightBox.anchoredPosition.y), boxMoveSpeed);

            mainBox.DOAnchorPos(new Vector2(mainBoxMidPos, mainBox.anchoredPosition.y), mainBoxSpeed);
        }

        private void Update()
        {
            if (gameStarted && gameManager.CurrentGameMode == GameMode.Test)
            {
                timer -= Time.unscaledDeltaTime;
                timerTxt.text = FormatTime(timer);
            }
        }

        string FormatTime(float rawTime)
        {
            int minutes = Mathf.FloorToInt(rawTime / 60F);
            int seconds = Mathf.FloorToInt(rawTime % 60F);

            // Use string.Format to ensure leading zeros (e.g., 05 instead of 5)
            string niceTime = string.Format("{0:00}:{1:00}", minutes, seconds);

            return niceTime;
        }
    }
}

