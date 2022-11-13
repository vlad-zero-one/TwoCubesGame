using UnityEngine;

namespace Assets.Scripts
{
    public class GameSceneController : MonoBehaviour
    {
        [SerializeField] private Menu menu;
        [SerializeField] private CubesController cubesController;
        [SerializeField] private PathController pathController;
        [SerializeField] private PathDragHandler pathDragHandler;
        [SerializeField] private SpheresController spheresController;

        private ScoreController scoreController = new ScoreController();

        private async void Start()
        {
            var gameSettings = DI.Get<GameSettings>();

            await spheresController.Init(pathDragHandler.gameObject);
            pathDragHandler.Init(gameSettings);
            pathController.Init(
                gameSettings, pathDragHandler, spheresController.StartSphere, spheresController.EndSphere);
            cubesController.Init(pathController, spheresController.StartSphere);
            scoreController.Init(cubesController, pathController);

            menu.Init(scoreController, StartGame, QuitGame);

            DI.Add(spheresController);
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

        private async void StartGame()
        {
            await spheresController.Restart();
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