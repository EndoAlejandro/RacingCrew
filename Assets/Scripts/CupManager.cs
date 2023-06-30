using System;
using System.Collections.Generic;
using CupComponents;
using CustomUtils;
using Menu.ScriptableObjects;

public class CupManager : Singleton<CupManager>
{
    public event Action<bool> OnLoading;
    public event Action TrackEnded;

    public List<CupRacer> CupRacers { get; private set; }

    private CupSelectionAssets _currentCup;
    private bool _playerResponse;
    private int _currentRaceIndex;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        CupRacers = new List<CupRacer>();
    }

    private void Start()
    {
        _currentCup = GameManager.Instance.CurrentCup;
        _currentRaceIndex = 0;

        CreateCupRacers();
        LoadTrack();
    }

    private void LoadTrack()
    {
        OnLoadingBegin();
        GameManager.Instance.LoadTrack(_currentCup.TracksData[_currentRaceIndex].sceneName, OnLoadingEnded);
    }

    private void OnLoadingBegin() => OnLoading?.Invoke(true);
    private void OnLoadingEnded() => OnLoading?.Invoke(false);

    private void CreateCupRacers()
    {
        var racersAmount = GameManager.Instance.FixedRacersAmount;
        for (int i = 0; i < racersAmount; i++)
        {
            var inputSingle = PlayersManager.Instance.TryGetPlayerInputSingle(i);
            var racer = new CupRacer(i, inputSingle);
            CupRacers.Add(racer);
        }
    }

    public void OnTrackEnded()
    {
        CupRacers.Sort();
        CupRacers.Reverse();
        TrackEnded?.Invoke();
        GameManager.Instance.LoadResultsScene();
    }

    public void LoadNextTrack()
    {
        _currentRaceIndex++;
        if (_currentRaceIndex < _currentCup.TracksData.Length)
            LoadTrack();
        else
            GameManager.Instance.LoadMainMenu();
    }
}