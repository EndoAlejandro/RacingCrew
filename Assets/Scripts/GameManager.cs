using System;
using System.Collections;
using CarComponents;
using CupComponents;
using CustomUtils;
using InputManagement;
using Menu.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public event Action<bool> OnGamePaused;

    [SerializeField] private int fixedRacersAmount = 5;
    [SerializeField] private CarData defaultCarData;

    public CupSelectionAssets CurrentCup { get; private set; }
    public int FixedRacersAmount => fixedRacersAmount;
    public CarData DefaultCarData => defaultCarData;
    public static bool IsGamePaused;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        OnGamePaused?.Invoke(true);
        IsGamePaused = true;
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        OnGamePaused?.Invoke(false);
        IsGamePaused = false;
        Time.timeScale = 1;
    }

    public void StartCup()
    {
        PlayersManager.Instance.SetState(PlayersManager.State.Race);
        StartCoroutine(StartCupAsync());
    }

    private IEnumerator StartCupAsync()
    {
        yield return SceneManager.LoadSceneAsync("Scenes/Cup");
    }

    public void LoadTrack(string sceneName, Action callback) =>
        StartCoroutine(LoadTrackAsync("Scenes/" + sceneName, callback));

    private IEnumerator LoadTrackAsync(string sceneName, Action callback)
    {
        if (SceneManager.loadedSceneCount > 1)
        {
            var last = SceneManager.GetSceneAt(SceneManager.loadedSceneCount - 1);
            yield return SceneManager.UnloadSceneAsync(last);
        }

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        callback?.Invoke();
    }

    public void LoadResultsScene() => StartCoroutine(LoadResultsSceneAsync());

    private IEnumerator LoadResultsSceneAsync()
    {
        if (SceneManager.loadedSceneCount > 1)
        {
            var last = SceneManager.GetSceneAt(SceneManager.loadedSceneCount - 1);
            yield return SceneManager.UnloadSceneAsync(last);
        }

        yield return SceneManager.LoadSceneAsync("Scenes/ScoreTable", LoadSceneMode.Additive);
    }

    public void SetCurrentCup(CupSelectionAssets cupSelectionAssets) => CurrentCup = cupSelectionAssets;

    public void LoadMainMenu()
    {
        if (CupManager.Instance != null) Destroy(CupManager.Instance.gameObject);
        PlayersManager.Instance.SetState(PlayersManager.State.WaitingFirstPlayer);
        SceneManager.LoadSceneAsync("Scenes/MainMenu");
    }
}