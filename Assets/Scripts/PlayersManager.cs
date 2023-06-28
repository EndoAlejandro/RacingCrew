using System;
using System.Collections.Generic;
using System.Linq;
using CustomUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersManager : Singleton<PlayersManager>
{
    public enum State
    {
        WaitingFirstPlayer,
        UI,
        Lobby,
        Race,
    }

    public event Action<State> OnStateChanged;
    public event Action<PlayerInput> OnPlayerJoined;
    public event Action<PlayerInput> OnPlayerDisconected;

    private PlayerInputManager _inputManager;
    private List<PlayerInput> _inputs;

    private State _state;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        _inputs = new List<PlayerInput>();
        _inputManager = GetComponent<PlayerInputManager>();
    }

    private void Start()
    {
        SetState(State.WaitingFirstPlayer);
        _inputManager.onPlayerJoined += InputManagerOnPlayerJoined;
    }

    public void OnLobby() => SetState(State.Lobby);

    public void SetState(State state)
    {
        _state = state;

        if (_state is State.WaitingFirstPlayer or State.Lobby) _inputManager.EnableJoining();
        else _inputManager.DisableJoining();

        OnStateChanged?.Invoke(_state);
    }

    private void InputManagerOnPlayerJoined(PlayerInput playerInput)
    {
        _inputs.Add(playerInput);
        OnPlayerJoined?.Invoke(playerInput);
        playerInput.onActionTriggered += PlayerInputOnActionTriggered;

        // if (_state == State.WaitingFirstPlayer) SetState(State.UI);
    }

    public void OnPlayerLost(PlayerInput playerInput)
    {
        _inputs.Remove(playerInput);
    }

    public void OnPlayerRegained(PlayerInput playerInput)
    {
        if (!_inputs.Contains(playerInput))
        {
            _inputs.Add(playerInput);
        }
    }

    private void PlayerInputOnActionTriggered(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log(context.action.name);
    }

    public PlayerInput GetPlayer(int index) =>
        _inputs.FirstOrDefault(input => input.playerIndex == index);
}