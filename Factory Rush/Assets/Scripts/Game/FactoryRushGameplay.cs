using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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
        [SerializeField] RectTransform leftBoxConveyerBelt;
        [SerializeField] RectTransform leftBox;
        [SerializeField] Vector2 leftBoxStartPos;
        [SerializeField] Vector2 leftBoxEndPos;
        [SerializeField] RectTransform rightBoxConveyerBelt;
        [SerializeField] RectTransform rightBox;
        [SerializeField] Vector2 rightBoxStartPos;
        [SerializeField] Vector2 rightBoxEndPos;
        List<int> options;
        [Header("Crane")]
        [SerializeField] RectTransform crane;
        [SerializeField] RectTransform craneOther;
        [SerializeField] Transform mainCanvas;
        [SerializeField] Vector2 craneStartPos;
        [SerializeField] int craneSiblingIndex;
        [SerializeField] float craneUpPosition;
        [Header("Main Belt")]
        [SerializeField] float mainBoxStartPos;
        [SerializeField] float mainBoxMidPos;
        [SerializeField] float mainBoxEndPos;
        [SerializeField] float mainBoxSpeed;
        [SerializeField] RectTransform mainBox;
        [SerializeField] GameObject trafficLightYellow;
        [SerializeField] GameObject trafficLightGreen;
        [Header("Questions")]
        [SerializeField] FactoryRushGameQuestions gameQuestions;
        [SerializeField] FactoryRushResult gameResult;

        bool responseCompleted;

        bool gameStarted;
        bool gameFinished;
        bool gameWon;

        int correctResponses;
        int wrongResponses;

        private void Awake()
        {
            gameManager.OnGameStart += HandleGameStart;
            gameQuestions.OnQuestionAnsweredCorrectly += StartNextQuestion;
        }

        private void OnDestroy()
        {
            gameManager.OnGameStart -= HandleGameStart;
            gameQuestions.OnQuestionAnsweredCorrectly -= StartNextQuestion;
        }

        private void Start()
        {
            MoveBoxesToStart(false);
        }

        void StartNextQuestion()
        {
            StartCoroutine(SetupNextQuestion());
        }

        void ShowResult(bool gameWon)
        {
            gameResult.ShowResultScreen(true);
            gameResult.SetupScoreTxts(correctResponses, wrongResponses);
            gameResult.SetupQuestionSummary(gameQuestions.CurrentQuestionDetails());
            if(gameWon)
                gameQuestions.NextQuestion();
        }

        public void RestartScene()
        {
            if(FactoryRushGameQuestions.currentQuestion == -1)
            {
                //Show Game Won Screen
                gameResult.ShowGameWonScreen();
                return;
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        IEnumerator SetupNextQuestion()
        {
            yield return new WaitUntil(() => responseCompleted);
            gameFinished = true;

            Debug.Log("Setting up next question");

            var mainBoxLid = mainBox.GetChild(0).GetComponent<RectTransform>();
            mainBoxLid.gameObject.SetActive(true); //Main Box Lid
            mainBoxLid.anchoredPosition = new Vector2(mainBoxLid.anchoredPosition.x, mainBoxLid.anchoredPosition.y + 50);
            mainBoxLid.DOAnchorPos(new Vector2(mainBoxLid.anchoredPosition.x, mainBoxLid.anchoredPosition.y - 50), 1f);

            yield return new WaitForSeconds(1);

            trafficLightYellow.gameObject.SetActive(false);
            trafficLightGreen.gameObject.SetActive(true);
            mainBox.DOAnchorPos(new Vector2(mainBoxEndPos, mainBox.anchoredPosition.y), mainBoxSpeed);
            
            yield return new WaitForSeconds(mainBoxSpeed + 1);

            ShowResult(true);
        }

        void MoveBoxesToStart(bool midRound = true)
        {
            gameFinished = false;
            responseCompleted = false;

            leftBox.SetParent(leftBoxConveyerBelt);
            rightBox.SetParent(rightBoxConveyerBelt);

            leftBox.GetComponent<CanvasGroup>().alpha = 1.0f;
            rightBox.GetComponent<CanvasGroup>().alpha = 1.0f;

            leftBox.anchoredPosition = leftBoxStartPos;
            rightBox.anchoredPosition = rightBoxStartPos;
            if (!midRound)
                mainBox.anchoredPosition = new Vector2(mainBoxStartPos, mainBox.anchoredPosition.y);

            trafficLightYellow.gameObject.SetActive(false);
            trafficLightGreen.gameObject.SetActive(false);

            SetupBoxesButtons(false);

            SetupCranes();

            if (!midRound)
                gameQuestions.SetupQuestion();
            options = gameQuestions.SetupOptions();
        }

        private void SetupCranes()
        {
            craneOther.gameObject.SetActive(false);

            crane.anchoredPosition = craneStartPos;
            craneOther.anchoredPosition = craneStartPos;

            crane.SetParent(mainCanvas);
            crane.SetSiblingIndex(craneSiblingIndex);
            craneOther.SetParent(mainCanvas);
            craneOther.SetSiblingIndex(craneSiblingIndex + 1);
        }

        void SetupBoxesButtons(bool state)
        {
            leftBox.GetComponent<Button>().enabled = state;
            rightBox.GetComponent<Button>().enabled = state;
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
            midText.text = "";
        }

        void MoveConveyerBeltBoxes()
        {
            leftBox.DOAnchorPos(leftBoxEndPos, boxMoveSpeed).OnComplete(() => SetupBoxesButtons(true));
            rightBox.DOAnchorPos(rightBoxEndPos, boxMoveSpeed);

            mainBox.DOAnchorPos(new Vector2(mainBoxMidPos, mainBox.anchoredPosition.y), mainBoxSpeed);

            trafficLightYellow.SetActive(true);
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

        IEnumerator Resetting()
        {
            responseCompleted = true;
            yield return new WaitForSeconds(0.25f);

            if (gameFinished)
                yield break;

            Debug.Log("Resetting");

            MoveBoxesToStart();
            yield return new WaitForSeconds(1);
            MoveConveyerBeltBoxes();
        }

        IEnumerator MoveCrane(RectTransform crane, RectTransform box, float waitTime = 1.5f)
        {
            yield return new WaitForSeconds(waitTime);
            crane.SetParent(box);
            crane.DOAnchorPos(new Vector2(0, crane.anchoredPosition.y), 1f).OnComplete(() =>
                {
                    crane.SetParent(mainCanvas);
                    crane.SetSiblingIndex(craneSiblingIndex);
                    box.SetParent(crane);
                    crane.DOAnchorPos(new Vector2(crane.anchoredPosition.x, craneUpPosition), 2f).OnComplete(() => StartCoroutine(Resetting()));
                });
        }

        void MoveBoxToMainBox(RectTransform box)
        {
            box.SetParent(mainBox);
            box.DOAnchorPos(Vector2.zero, 2f);
            box.GetComponent<CanvasGroup>().DOFade(0, 1f).SetDelay(1f);
        }

        //0 - Right Option
        //1 - Wrong Option

        public void OnClickLeftOption()
        {
            if (options[0] == 0)
            {
                correctResponses++;

                MoveBoxToMainBox(leftBox);
                StartCoroutine(MoveCrane(crane, rightBox));
                gameQuestions.RemoveCorrectOptionAfterRightAnswer();
            }
            else
            {
                wrongResponses++;

                craneOther.gameObject.SetActive(true);
                StartCoroutine(MoveCrane(crane, leftBox, 0));
                StartCoroutine(MoveCrane(craneOther, rightBox, 0));
                if (gameManager.CurrentGameMode == GameMode.Test)
                    RemoveLife();

            }
            SetupBoxesButtons(false);
        }

        public void OnClickRightOption()
        {
            if (options[1] == 0)
            {
                correctResponses++;

                MoveBoxToMainBox(rightBox);
                StartCoroutine(MoveCrane(crane, leftBox));
                gameQuestions.RemoveCorrectOptionAfterRightAnswer();
            }
            else
            {
                wrongResponses++;

                craneOther.gameObject.SetActive(true);
                StartCoroutine(MoveCrane(crane, rightBox, 0));
                StartCoroutine(MoveCrane(craneOther, leftBox, 0));
                if (gameManager.CurrentGameMode == GameMode.Test)
                    RemoveLife();
            }
            SetupBoxesButtons(false);
        }

        void RemoveLife()
        {
            for (int i = hearts.Length - 1; i >= 0; i--)
            {
                if (hearts[i].gameObject.activeSelf)
                {
                    hearts[i].gameObject.SetActive(false);
                    return;
                }
            }

            gameFinished = true;
            ShowResult(false);
        }
    }
}

