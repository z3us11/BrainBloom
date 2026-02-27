using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Eduzo.Games.FactoryRush
{
    public class FactoryRushForm : MonoBehaviour
    {
        [SerializeField] FactoryRushFormQuestions formQuestionGameobject;
        [SerializeField] RectTransform formQuestionsParent;
        [Header("Saving")]
        [SerializeField] FactoryRushSaveSystem saveSystem;
        [SerializeField] TMP_Text logMsg;

        int totalQuestions = 1;

        private void Start()
        {
            AddQuestion();

            logMsg.text = "";
        }

        public void AddQuestion()
        {
            var question = Instantiate(formQuestionGameobject, formQuestionsParent.transform);
            question.Init(totalQuestions++);

            saveSystem.AddQuestion(question.GetSavedQuestion());

            LayoutRebuilder.ForceRebuildLayoutImmediate(formQuestionsParent);
        }

        public void SaveQuestions()
        {
            StartCoroutine(ShowMsg(saveSystem.SaveQuestions()));
        }
        public void LoadQuestions()
        {
            StartCoroutine(ShowMsg(saveSystem.LoadQuestions()));
            SpawnLoadedQuestions();
        }

        void SpawnLoadedQuestions()
        {
            totalQuestions = 1;

            for (int i = 0; i < formQuestionsParent.childCount; i++)
            {
                Destroy(formQuestionsParent.GetChild(i).gameObject);
            }

            List<SavedQuestions> savedQuestions = saveSystem.GetLoadedQuestions();
            for(int i = 0; i < savedQuestions.Count; i++)
            {
                var question = Instantiate(formQuestionGameobject, formQuestionsParent.transform);
                question.Init(totalQuestions++, savedQuestions[i]);
            }
        }

        IEnumerator ShowMsg(string msg)
        {
            logMsg.text = msg;

            yield return new WaitForSeconds(3f);
            logMsg.text = "";
        }

        public void StartGame()
        {
            SceneManager.LoadScene("FactoryRushGameplayScene");
        }
    }
}

