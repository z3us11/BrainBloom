using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace Eduzo.Games.FactoryRush
{
    public class FactoryRushGameManager : MonoBehaviour
    {
        [SerializeField] GameObject gameModeSelectPanel;
        [SerializeField] GameObject gamePanel;
        [SerializeField] Button practiceModeBtn;
        [SerializeField] Button testModeBtn;
        [Header("Game Mode Elements")]
        [SerializeField] GameObject testModeUI;

        public UnityAction OnGameStart;

        GameMode currentGameMode;
        public GameMode CurrentGameMode => currentGameMode;

        private void Start()
        {
            gameModeSelectPanel.SetActive(true);
            gamePanel.SetActive(false);

            practiceModeBtn.onClick.AddListener(() => StartGame(GameMode.Practice));
            testModeBtn.onClick.AddListener(() => StartGame(GameMode.Test));
        }

        public void StartGame(GameMode gameMode)
        {
            currentGameMode = gameMode;

            gameModeSelectPanel.SetActive(false);
            gamePanel.SetActive(true);

            if (currentGameMode == GameMode.Test)
            {
                testModeUI.SetActive(true);
            }
            else
            {
                testModeUI.SetActive(false);
            }

            OnGameStart?.Invoke();
        }
    }

    public enum GameMode
    {
        Practice,
        Test
    }

}

