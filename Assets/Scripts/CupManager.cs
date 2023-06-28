using System;
using System.Collections.Generic;
using CarComponents;
using CupComponents;
using CustomUtils;
using Menu.ScriptableObjects;

public class CupManager : Singleton<CupManager>
{
    public event Action<bool> OnLoading; 
    public int CurrentRaceIndex { get; private set; }
    public List<CupRacer> Racers { get; private set; }

    private List<PlayerControllerInput> _playersController;
    private CupSelectionAssets _currentCup;
    private bool _playerResponse;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        Racers = new List<CupRacer>();
    }

    private void Start()
    {
        _currentCup = GameManager.Instance.CurrentCup;
        CurrentRaceIndex = 0;

        CreateCupRacers();
        OnLoadingBegin();
        GameManager.Instance.LoadTrack(_currentCup.TracksData[0].sceneName, OnLoadingEnded);
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
            Racers.Add(racer);
        }
    }
}