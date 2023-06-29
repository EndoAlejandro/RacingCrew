using System.Collections.Generic;
using CarComponents;
using CustomUtils;
using InGame;
using Menu;
using RaceComponents;
using UnityEngine;
using UnityEngine.InputSystem;

public class CupManager : Singleton<CupManager>
{
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private Camera loadingCamera;


    private PlayerInputManager _inputManager;
    private CupSelectionAssets _currentCup;

    private List<PlayerControllerInput> _playersController;

    private bool _playerResponse;

    public int CurrentRaceIndex { get; private set; }
    public List<Racer> Racers { get; private set; } = new List<Racer>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        _inputManager = GetComponent<PlayerInputManager>();
    }

    private void Start()
    {
        _currentCup = GameManager.Instance.CurrentCup;
        CurrentRaceIndex = 0;

        OnLoadingBegin();
        GameManager.Instance.LoadTrack(_currentCup.TracksData[0].sceneName, OnLoadingEnded);
    }

    private void OnLoadingEnded()
    {
        loadingCanvas.gameObject.SetActive(false);
        loadingCamera.enabled = false;

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
    }

    private AiControllerInput CreateNewAiController(int index)
    {
        var controller = new GameObject("AIController_" + index).AddComponent<AiControllerInput>();
        controller.Setup(index);
        return controller;
    }

    private void OnLoadingBegin()
    {
        // CreatePlayersInput();
        // var playerInput = PlayersManager.Instance.GetPlayer(0);
        
        loadingCanvas.gameObject.SetActive(true);
        loadingCamera.enabled = true;
    }

    private void CreatePlayersInput()
    {
        var players = GameManager.Instance.PlayerCarInformation;
        _inputManager.EnableJoining();

        foreach (var player in players)
            _inputManager.JoinPlayer(controlScheme: player.Scheme);

        _inputManager.DisableJoining();
    }
}