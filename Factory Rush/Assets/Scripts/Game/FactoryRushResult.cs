using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Eduzo.Games.FactoryRush
{
    public class FactoryRushResult : MonoBehaviour
    {
        [Header("Result Summary")]
        [SerializeField] TMP_Text scoreTxt;
        [SerializeField] TMP_Text totalResponsesTxt;
        [SerializeField] TMP_Text correctResponsesTxt;
        [SerializeField] TMP_Text wrongResponsesTxt;
        [Header("Question Summary")]
        [SerializeField] TMP_Text questionTxt;
        [SerializeField] TMP_Text rightAnswersTxt;
        [SerializeField] TMP_Text wrongAnswersTxt;
        [Header("NextButton")]
        [SerializeField] TMP_Text nextButtonTxt;
        [SerializeField] GameObject gameWonScreen;

        public void ShowResultScreen(bool gameWon)
        {
            gameObject.SetActive(true);
            nextButtonTxt.text = gameWon ? "Next Question" : "Retry";
        }

        public void ShowGameWonScreen()
        {
            gameWonScreen.SetActive(true);
        }

        public void SetupScoreTxts(int correctResponses, int wrongResponses)
        {
            int totalResponses = correctResponses + wrongResponses;
            float score = (correctResponses * 1.0f/ totalResponses * 1.0f) * 100;
            scoreTxt.text = string.Format("Score: {0:0.00} %", score);

            totalResponsesTxt.text = string.Format("Total Responses: {0}", totalResponses);
            correctResponsesTxt.text = string.Format("Correct Responses: {0}", correctResponses);
            wrongResponsesTxt.text = string.Format("Wrong Responses: {0}", wrongResponses);
        }

        public void SetupQuestionSummary(SavedQuestions currentQuestionDetails)
        {
            questionTxt.text = "Question: " + currentQuestionDetails.question;

            rightAnswersTxt.text = "Right Answers: ";
            for (int i = 0; i < currentQuestionDetails.rightAnswers.Count; i++)
            {
                string add = ", ";
                if (i == currentQuestionDetails.rightAnswers.Count - 1)
                    add = "";
                rightAnswersTxt.text += currentQuestionDetails.rightAnswers[i] + add;
            }

            wrongAnswersTxt.text = "Wrong Answers: ";
            for (int i = 0; i < currentQuestionDetails.wrongAnswers.Count; i++)
            {
                string add = ", ";
                if (i == currentQuestionDetails.wrongAnswers.Count - 1)
                    add = "";
                wrongAnswersTxt.text += currentQuestionDetails.wrongAnswers[i] + add;
            }
        }
    }
}

