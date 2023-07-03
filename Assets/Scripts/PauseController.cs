using CupComponents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TMP_Text cupText;
    [SerializeField] private TMP_Text trackText;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button mainMenuButton;

    private void Start()
    {
        GameManagerOnGamePaused(false);
        GameManager.Instance.OnGamePaused += GameManagerOnGamePaused;

        continueButton.onClick.AddListener(ContinueButtonPressed);
        mainMenuButton.onClick.AddListener(MainMenuButtonPressed);
    }

    private void GameManagerOnGamePaused(bool isPaused)
    {
        var currentCup = GameManager.Instance.CurrentCup;
        cupText.SetText(currentCup.CupName);
        var trackIndex = CupManager.Instance.CurrentRaceIndex;
        trackText.SetText(currentCup.TracksData[trackIndex].displayName);

        pausePanel.SetActive(isPaused);
    }

    private void ContinueButtonPressed() => GameManager.Instance.UnpauseGame();

    private void MainMenuButtonPressed()
    {
        ContinueButtonPressed();
        GameManager.Instance.LoadMainMenu();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null) return;
        GameManager.Instance.OnGamePaused -= GameManagerOnGamePaused;
    }
}