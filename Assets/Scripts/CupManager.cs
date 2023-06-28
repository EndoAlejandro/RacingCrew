using System.Collections.Generic;
using CarComponents;
using CupComponents;
using CustomUtils;
using Menu;
using Menu.ScriptableObjects;
using RaceComponents;
using UnityEngine;

public class CupManager : Singleton<CupManager>
{
    [Header("Loading")]
    [SerializeField] private GameObject loadingCanvas;

    [SerializeField] private Camera loadingCamera;

    [Header("Controllers")]
    [SerializeField] private PlayerControllerInput playerControllerInputPrefab;

    private List<CupRacer> _racers;

    private List<PlayerControllerInput> _playersController;

    private CupSelectionAssets _currentCup;
    private bool _playerResponse;

    public List<Racer> Racers { get; private set; } = new List<Racer>();
    public int CurrentRaceIndex { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);

        _racers = new List<CupRacer>();
    }

    private void Start()
    {
        _currentCup = GameManager.Instance.CurrentCup;
        CurrentRaceIndex = 0;

        CreateCupRacers();
        OnLoadingBegin();
        GameManager.Instance.LoadTrack(_currentCup.TracksData[0].sceneName, OnLoadingEnded);
    }

    private void OnLoadingEnded()
    {
        loadingCanvas.gameObject.SetActive(false);
        loadingCamera.enabled = false;
        /*
        var racersAmount = GameManager.Instance.FixedRacersAmount;
        var carData = GameManager.Instance.DefaultCarData;

        for (int i = 0; i < racersAmount; i++)
        {
            var model = carData.Models[Random.Range(0, carData.Models.Length)];
            IControllerInput controllerInput = CreateNewAiController(i);

            var racer = new Racer(carData, model, controllerInput);
            Racers.Add(racer);
        }

        var players = FindObjectsOfType<PlayerControllerInput>();
        foreach (var player in players)
        {
            var index = player.PlayerInput.playerIndex;
            var racer = Racers[index];
            var model = GameManager.Instance.PlayerCarInformation[index].CarModel;
            racer.SetControllerInput(player);
            racer.SetCarModel(model);
        }
        */
    }

    private AiControllerInput CreateNewAiController(int index)
    {
        var controller = new GameObject("AIController_" + index).AddComponent<AiControllerInput>();
        controller.Setup(index);
        return controller;
    }

    private void OnLoadingBegin()
    {
        loadingCanvas.gameObject.SetActive(true);
        loadingCamera.enabled = true;
    }

    private void CreateCupRacers()
    {
        var racersAmount = GameManager.Instance.FixedRacersAmount;
        for (int i = 0; i < racersAmount; i++)
        {
            var inputSingle = PlayersManager.Instance.TryGetPlayerInputSingle(i);
            var racer = new CupRacer(i, inputSingle);
            _racers.Add(racer);
        }
    }
}