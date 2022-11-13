using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class GameSceneController : MonoBehaviour
    {
        [SerializeField] private Menu menu;
        [SerializeField] private CubesController cubesController;
        [SerializeField] private PathController pathController;
        [SerializeField] private PathDragHandler pathDragHandler;

        private ScoreController scoreController = new ScoreController();

        private void Start()
        {
            var gameSettings = DI.Get<GameSettings>();

            pathDragHandler.Init(gameSettings);
            pathController.Init(gameSettings, pathDragHandler);
            cubesController.Init(pathController);
            scoreController.Init(cubesController, pathController);

            menu.Init(scoreController, StartGame, QuitGame);

            DI.Add(pathController);
            DI.Add(cubesController);
            DI.Add(scoreController);

            cubesController.OnCubesTouched.AddListener(LostGame);
            cubesController.OnAllCubesReachedEndSphere.AddListener(WinGame);

        }

        private void WinGame()
        {
            menu.SetLoseWinText(win: true);
            menu.SetActive(true);
        }

        private void LostGame()
        {
            menu.SetLoseWinText(win: false);
            menu.SetActive(true);
        }

        private void StartGame()
        {
            pathController.Restart();
            cubesController.Restart();

            menu.SetActive(false);
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                menu.SetActive(!menu.gameObject.activeSelf);
            }
        }

        private void OnDestroy()
        {
            cubesController.OnCubesTouched.RemoveListener(LostGame);
            cubesController.OnAllCubesReachedEndSphere.RemoveListener(WinGame);
        }
    }
}