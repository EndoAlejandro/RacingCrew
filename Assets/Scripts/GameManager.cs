using System;
using System.Collections;
using System.Collections.Generic;
using CarComponents;
using CupComponents;
using CustomUtils;
using InGame;
using Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public event Action<bool> OnGamePaused;

    [SerializeField] private int fixedRacersAmount = 5;
    [SerializeField] private CarData defaultCarData;
    [SerializeField] private AllCupsAssets allCupsAssets;

    private Dictionary<int, GrandPrixRacer> _cupRacers = new();
    public List<ModelAndInputPlayer> PlayerCarInformation { get; private set; } = new();
    public CupSelectionAssets CurrentCup { get; private set; }

    public int CurrentSpeedway { get; private set; }
    public int FixedRacersAmount => fixedRacersAmount;
    public CarData DefaultCarData => defaultCarData;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void AddPlayerCarModel(PlayerInput input, GameObject model) =>
        PlayerCarInformation.Add(new ModelAndInputPlayer(input, model));

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

    public void LoadTrack(string sceneName, Action turnOff) =>
        StartCoroutine(LoadTrackAsync(sceneName, turnOff));

    private IEnumerator LoadTrackAsync(string sceneName, Action turnOff)
    {
        if (SceneManager.loadedSceneCount > 1)
        {
            var last = SceneManager.GetSceneAt(SceneManager.loadedSceneCount);
            yield return SceneManager.UnloadSceneAsync(last);
        }

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        turnOff?.Invoke();
    }

    public void LoadNextSpeedway()
    {
        CurrentSpeedway++;
        SceneManager.LoadScene(CurrentCup.TracksData[CurrentSpeedway - 1].sceneName);
    }

    public void SetCurrentCup(CupSelectionAssets cupSelectionAssets) => CurrentCup = cupSelectionAssets;
}