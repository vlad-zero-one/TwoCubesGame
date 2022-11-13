using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button quitGameButton;

        [SerializeField] private Text bestScore;
        [SerializeField] private Text lastScore;

        [SerializeField] private Text loseWinTextObject;

        [SerializeField] private string LoseText;
        [SerializeField] private string WinText;

        private ScoreController scoreController;

        public void Init(ScoreController scoreController,
            UnityAction onStartButtonPressed,
            UnityAction onQuitGameButtonPressed)
        {
            this.scoreController = scoreController;

            startButton.onClick.AddListener(onStartButtonPressed);
            quitGameButton.onClick.AddListener(onQuitGameButtonPressed);
        }

        public void SetLoseWinText(bool win = true)
        {
            loseWinTextObject.text = win ? WinText : LoseText;
        }

        public void SetActive(bool value)
        {
            if (value) Show();
            else Hide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
            bestScore.text = scoreController.GetBestScore().ToString();
            lastScore.text = scoreController.GetLastScore().ToString();
        }

        private void Hide()
        {
            gameObject.SetActive(false);
            loseWinTextObject.text = string.Empty;
        }

        private void OnDestroy()
        {
            startButton.onClick.RemoveAllListeners();
            quitGameButton.onClick.RemoveAllListeners();
        }
    }
}