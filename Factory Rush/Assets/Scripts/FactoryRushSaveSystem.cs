using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace Eduzo.Games.FactoryRush
{
    public class FactoryRushSaveSystem : MonoBehaviour
    {
        Save save;

        private void Start()
        {
            save = new Save();
        }

        public void AddQuestion(SavedQuestions question)
        {
            save.allQuestions.Add(question);
        }

        public List<SavedQuestions> GetLoadedQuestions()
        {
            return save.allQuestions;
        }

        public string SaveQuestions()
        {
            var formQuestionsJson = JsonUtility.ToJson(save);

            string path = Application.persistentDataPath + "/gamesave.json";
            File.WriteAllText(path, formQuestionsJson);

            Debug.Log(formQuestionsJson);

            return "Questions Saved!";

        }
        public string LoadQuestions()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "gamesave.json");

            if (File.Exists(filePath))
            {
                // 1. Read the file content into a string
                string jsonString = File.ReadAllText(filePath);

                save = new Save();
                save = JsonUtility.FromJson<Save>(jsonString);

                return "Questions Loaded!";
            }
            else
            {
                return "Save file not found at " + filePath;
            }
        }
    }

    [Serializable]
    public class Save
    {
        public List<SavedQuestions> allQuestions = new List<SavedQuestions>();
    }

    [Serializable]
    public class SavedQuestions
    {
        public string question = "";
        public List<string> rightAnswers = new List<string>();
        public List<string> wrongAnswers = new List<string>();

        public SavedQuestions()
        {
            question = string.Empty;
            rightAnswers = new List<string>();
            wrongAnswers = new List<string>();
        }
    }

    [Serializable]
    public class Responses
    {
        public int totalQuestionAnswered = 0;
        public int correctAnswerAnswered = 0;
        public int wrongAnswerAnswered = 0;

        public Responses()
        {
            totalQuestionAnswered = 0;
            correctAnswerAnswered = 0;
            wrongAnswerAnswered = 0;
        }
    }
}

