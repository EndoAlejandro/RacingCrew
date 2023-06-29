using CustomUtils;
using UnityEngine;

public class VehicleInputReader : IInputReader, IControllerInput
{
    public float Acceleration { get; private set; }
    public float Turn { get; private set; }
    public float Break { get; private set; }

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
            default:
                Debug.LogError($"The action '{actionName}' is not supported.");
                break;
        }
    }
}