using CustomUtils;
using UnityEngine;

namespace InputManagement
{
    public class VehicleInputReader : IInputReader, ICarControllerInput
    {
        public float Acceleration { get; private set; }
        public float Turn { get; private set; }
        public float Break { get; private set; }
        public bool ResetPosition { get; private set; }
        public bool Pause { get; private set; }

        public void ReadInput(string actionName, UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            switch (actionName)
            {
                case VehicleInput.ACCELERATION: // button.
                    Acceleration = context.ReadValue<float>();
                    break;
                case VehicleInput.TURNING: // Vector2.
                    Turn = context.ReadValue<float>();
                    break;
                case VehicleInput.BREAK: // button.
                    Break = context.ReadValue<float>();
                    break;
                case VehicleInput.RESET_POSITION:
                    ResetPosition = context.ReadValueAsButton();
                    break;
                case VehicleInput.PAUSE:
                    Pause = context.ReadValueAsButton();
                    if (Pause && !GameManager.IsGamePaused) GameManager.Instance.PauseGame();
                    break;
                default:
                    Debug.LogError($"The action '{actionName}' is not supported.");
                    break;
            }
        }
    }
}