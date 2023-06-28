using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputSingle : MonoBehaviour
{
    private PlayerInput _input;

    private void Awake() => _input = GetComponent<PlayerInput>();

    private void Start()
    {
        transform.SetParent(PlayersManager.Instance.transform);
        
        PlayersManager.Instance.OnStateChanged += PlayersManagerOnStateChanged;
        _input.onDeviceLost += InputOnDeviceLost;
        _input.onDeviceRegained += InputOnDeviceRegained;
    }

    private void PlayersManagerOnStateChanged(PlayersManager.State state) =>
        SwitchActionMap(state == PlayersManager.State.Race ? InputAction.Vehicle : InputAction.MainMenu);

    public void SwitchActionMap(InputAction action) => _input.SwitchCurrentActionMap(action.ToString());

    private void InputOnDeviceRegained(PlayerInput input) =>
        PlayersManager.Instance.OnPlayerRegained(input);

    private void InputOnDeviceLost(PlayerInput input) =>
        PlayersManager.Instance.OnPlayerLost(input);

    private void OnDestroy()
    {
        _input.onDeviceLost -= InputOnDeviceLost;
        _input.onDeviceRegained -= InputOnDeviceRegained;

        if (PlayersManager.Instance == null) return;
        PlayersManager.Instance.OnStateChanged -= PlayersManagerOnStateChanged;
    }
}

public enum InputAction
{
    MainMenu,
    Vehicle,
}