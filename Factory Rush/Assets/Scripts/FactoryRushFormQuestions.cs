using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Eduzo.Games.FactoryRush
{
    public class FactoryRushFormQuestions : MonoBehaviour
    {
        [SerializeField] TMP_Text questionTxt;
        [SerializeField] TMP_InputField questionInputField;
        [SerializeField] TMP_Text rightAnswersTxt;
        [SerializeField] TMP_InputField rightAnswersInputField;
        [SerializeField] TMP_Text wrongAnswersTxt;
        [SerializeField] TMP_InputField wrongAnswersInputField;

        string defaultQuestionTxt = "Question {0}: ";
        string defaultRightAnswersTxt = "Right Answer(s): ";
        string defaultWrongAnswersTxt = "Wrong Answer(s): ";

        int questionNumber;
        SavedQuestions savedQuestion;

        string question = "";
        List<string> rightAnswers = new List<string>();
        List<string> wrongAnswers = new List<string>();

        public SavedQuestions GetSavedQuestion()
        {
            return savedQuestion;
        }

        public void Init(int _questionNumber, SavedQuestions _savedQuestion = null)
        {
            savedQuestion = new SavedQuestions();

            questionNumber = _questionNumber;
            questionTxt.text = string.Format(defaultQuestionTxt, questionNumber);
            rightAnswersTxt.text = defaultRightAnswersTxt;
            wrongAnswersTxt.text = defaultWrongAnswersTxt;

            if(_savedQuestion != null)
            {
                question = _savedQuestion.question;
                rightAnswers = new List<string>();
                rightAnswers = _savedQuestion.rightAnswers;
                wrongAnswers = new List<string>();
                wrongAnswers = _savedQuestion.wrongAnswers;

                questionTxt.text = string.Format(defaultQuestionTxt, questionNumber) + _savedQuestion.question;
                rightAnswersTxt.text = defaultRightAnswersTxt + GetRighAnswerString();
                wrongAnswersTxt.text = defaultWrongAnswersTxt + GetWrongAnswerString();
            }
        }

        public void OnQuestionUpdated()
        {
            question = questionInputField.text;
            questionTxt.text = string.Format(defaultQuestionTxt, questionNumber) + question;

            savedQuestion.question = question;
        }

        public void OnRightAnswersUpdated()
        {
            if (string.IsNullOrEmpty(rightAnswersInputField.text))
                return;

            rightAnswers.Add(rightAnswersInputField.text);
            string rightAnswersString = GetRighAnswerString();
            rightAnswersTxt.text = defaultRightAnswersTxt + rightAnswersString;

            savedQuestion.rightAnswers.Add(rightAnswersInputField.text);
        }

        private string GetRighAnswerString()
        {
            string rightAnswersString = "";
            for (int i = 0; i < rightAnswers.Count; i++)
            {
                if (i == 0)
                    rightAnswersString += rightAnswers[i];
                else
                    rightAnswersString += ", " + rightAnswers[i];
            }

            return rightAnswersString;
        }

        public void OnWrongAnswersUpdated()
        {
            if (string.IsNullOrEmpty(wrongAnswersInputField.text))
                return;

            wrongAnswers.Add(wrongAnswersInputField.text);
            string wrongAnswersString = GetWrongAnswerString();
            wrongAnswersTxt.text = defaultWrongAnswersTxt + wrongAnswersString;

            savedQuestion.wrongAnswers.Add(wrongAnswersInputField.text);
        }

        private string GetWrongAnswerString()
        {
            string wrongAnswersString = "";
            for (int i = 0; i < wrongAnswers.Count; i++)
            {
                if (i == 0)
                    wrongAnswersString += wrongAnswers[i];
                else
                    wrongAnswersString += ", " + wrongAnswers[i];
            }

            return wrongAnswersString;
        }
    }
}

