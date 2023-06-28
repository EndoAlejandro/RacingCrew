using System;
using System.Collections;
using CarComponents;
using CustomUtils;
using Menu;
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
        Time.timeScale = 0;
    }

    public void ContinueRace()
    {
        OnGamePaused?.Invoke(false);
        Time.timeScale = 1;
    }

    public void StartCup() => StartCoroutine(StartCupAsync());

    private IEnumerator StartCupAsync()
    {
        yield return SceneManager.LoadSceneAsync("Scenes/Cup");
    }

    public void LoadTrack(string sceneName, Action callback) =>
        StartCoroutine(LoadTrackAsync(sceneName, callback));

    private IEnumerator LoadTrackAsync(string sceneName, Action callback)
    {
        if (SceneManager.loadedSceneCount > 1)
        {
            var last = SceneManager.GetSceneAt(SceneManager.loadedSceneCount);
            yield return SceneManager.UnloadSceneAsync(last);
        }

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        callback?.Invoke();
    }

    public void SetCurrentCup(CupSelectionAssets cupSelectionAssets) => CurrentCup = cupSelectionAssets;
}