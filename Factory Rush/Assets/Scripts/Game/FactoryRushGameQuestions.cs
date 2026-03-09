using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


namespace Eduzo.Games.FactoryRush
{
    public class FactoryRushGameQuestions : MonoBehaviour
    {
        [SerializeField] TMP_Text option1Txt;
        [SerializeField] TMP_Text option2Txt;
        [SerializeField] TMP_Text questionTxt;

        public static int currentQuestion;
        List<SavedQuestions> allSavedQuestions;

        List<string> rightAnswers = new List<string>();
        List<string> wrongAnswers = new List<string>();

        int currentRightAnswerIndex = -1;

        public UnityAction OnQuestionAnsweredCorrectly;

        private void Awake()
        {
            allSavedQuestions = new List<SavedQuestions>();
            allSavedQuestions = FactoryRushSaveSystem.Instance.Save.allQuestions;
        }

        public SavedQuestions CurrentQuestionDetails()
        {
            return allSavedQuestions[currentQuestion];
        }

        public void SetupQuestion()
        {
            if (currentQuestion >= allSavedQuestions.Count)
                return;
            questionTxt.text = allSavedQuestions[currentQuestion].question;

            rightAnswers = new List<string>(allSavedQuestions[currentQuestion].rightAnswers);
            wrongAnswers = new List<string>(allSavedQuestions[currentQuestion].wrongAnswers);
        }

        public List<int> SetupOptions()
        {
            if (currentQuestion >= allSavedQuestions.Count)
                return new List<int>();

            int randomRightAnswerIndex = Random.Range(0, rightAnswers.Count);
            int randomWrongAnswerIndex = Random.Range(0, wrongAnswers.Count);

            currentRightAnswerIndex = randomRightAnswerIndex;

            List<int> options = new List<int>();
            options.Clear();

            //0 - Right Option
            //1 - Wrong Option

            if (Random.value > 0.5)
            {
                option1Txt.text = rightAnswers[randomRightAnswerIndex];
                option2Txt.text = wrongAnswers[randomWrongAnswerIndex];
                options.Add(0);
                options.Add(1);
            }
            else
            {
                option2Txt.text = rightAnswers[randomRightAnswerIndex];
                option1Txt.text = wrongAnswers[randomWrongAnswerIndex];
                options.Add(1);
                options.Add(0);
            }

            return options;
        }

        public void RemoveCorrectOptionAfterRightAnswer()
        {
            rightAnswers.RemoveAt(currentRightAnswerIndex);
            currentRightAnswerIndex = -1;

            if (rightAnswers.Count <= 0)
            {
                OnQuestionAnsweredCorrectly?.Invoke();
            }
        }

        public void NextQuestion()
        {
            currentQuestion++;
            if(currentQuestion >= allSavedQuestions.Count)
            {
                currentQuestion = -1;
            }
        }
    }
}

