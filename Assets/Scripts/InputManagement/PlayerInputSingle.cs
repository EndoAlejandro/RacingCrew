using System;
using CarComponents;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManagement
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputSingle : MonoBehaviour
    {
        [SerializeField] private CarData defaultCarData;

        public event Action OnInputTriggered;
        public PlayerInput Input { get; private set; }
        public MainMenuInputReader MainMenuInputReader { get; private set; } = new();
        public VehicleInputReader VehicleInputReader { get; private set; } = new();
        public int PlayerIndex => Input != null ? Input.playerIndex : OnPlayerIndexNull();
        public GameObject CarModel { get; private set; }
        public CarStats CarStats { get; private set; }

        private IInputReader _currentInputReader;

        private void Awake()
        {
            Input = GetComponent<PlayerInput>();
            CarStats = defaultCarData.Stats;
        }

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

        public void SetModelIndex(GameObject value) => CarModel = value;
        public void SetCarData(CarStats value) => CarStats = value;
    }
}