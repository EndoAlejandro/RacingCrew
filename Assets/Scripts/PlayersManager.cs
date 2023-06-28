using System;
using System.Collections.Generic;
using System.Linq;
using CustomUtils;
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
    public event Action<PlayerInputSingle> OnPlayerJoined;
    public event Action<PlayerInputSingle> OnPlayerDisconnected;

    private PlayerInputManager _inputManager;
    public List<PlayerInputSingle> PlayerInputs { get; private set; }

    public State CurrentState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        PlayerInputs = new List<PlayerInputSingle>();
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
        CurrentState = state;

        if (CurrentState is State.Lobby) _inputManager.EnableJoining();
        else _inputManager.DisableJoining();

        OnStateChanged?.Invoke(CurrentState);
    }

    private void InputManagerOnPlayerJoined(PlayerInput playerInput)
    {
        if (!playerInput.TryGetComponent(out PlayerInputSingle single)) return;
        PlayerInputs.Add(single);
        OnPlayerJoined?.Invoke(single);

        if (CurrentState == State.WaitingFirstPlayer) SetState(State.UI);
    }

    public void OnPlayerLost(PlayerInputSingle single)
    {
        PlayerInputs.Remove(single);
        OnPlayerDisconnected?.Invoke(single);
    }

    public void OnPlayerRegained(PlayerInputSingle single)
    {
        if (PlayerInputs.Contains(single)) return;
        PlayerInputs.Add(single);
        OnPlayerJoined?.Invoke(single);
    }

    public PlayerInputSingle GetPlayer(int index) =>
        PlayerInputs.FirstOrDefault(single => single.Input.playerIndex == index);
}