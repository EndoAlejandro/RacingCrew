using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputSingle : MonoBehaviour
{
    public event Action OnInputTriggered;
    public PlayerInput Input { get; private set; }
    public MainMenuInputReader MainMenuInputReader { get; private set; } = new();
    public VehicleInputReader VehicleInputReader { get; private set; } = new();
    private IInputReader _currentInputReader;
    public int PlayerIndex => Input != null ? Input.playerIndex : OnPlayerIndexNull();

    private void Awake() => Input = GetComponent<PlayerInput>();

    private int OnPlayerIndexNull()
    {
        Input = GetComponent<PlayerInput>();
        return PlayerIndex;
    }

    private void Start()
    {
        PlayersManagerOnStateChanged(PlayersManager.Instance.CurrentState);
        transform.SetParent(PlayersManager.Instance.transform);

        PlayersManager.Instance.OnStateChanged += PlayersManagerOnStateChanged;
        Input.onDeviceLost += InputOnDeviceLost;
        Input.onDeviceRegained += InputOnDeviceRegained;
        Input.onActionTriggered += InputOnActionTriggered;
    }

    private void InputOnActionTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (_currentInputReader == null) Debug.LogError("CurrentInputReader is null.");
        else _currentInputReader.ReadInput(context.action.name, context);
        OnInputTriggered?.Invoke();
    }

    private void PlayersManagerOnStateChanged(PlayersManager.State state)
    {
        if (state == PlayersManager.State.Race)
        {
            SwitchActionMap(InputAction.Vehicle);
            _currentInputReader = VehicleInputReader;
        }
        else
        {
            SwitchActionMap(InputAction.MainMenu);
            _currentInputReader = MainMenuInputReader;
        }
    }

    private void SwitchActionMap(InputAction action) => Input.SwitchCurrentActionMap(action.ToString());

    private void InputOnDeviceRegained(PlayerInput input) =>
        PlayersManager.Instance.OnPlayerRegained(this);

    private void InputOnDeviceLost(PlayerInput input) =>
        PlayersManager.Instance.OnPlayerLost(this);

    private void OnDestroy()
    {
        Input.onDeviceLost -= InputOnDeviceLost;
        Input.onDeviceRegained -= InputOnDeviceRegained;
        Input.onActionTriggered -= InputOnActionTriggered;

        if (PlayersManager.Instance == null) return;
        PlayersManager.Instance.OnStateChanged -= PlayersManagerOnStateChanged;
    }
}