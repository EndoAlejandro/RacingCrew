using CustomUtils;
using UnityEngine;

public class MainMenuInputReader : IInputReader
{
    public Vector2 Navigation { get; private set; }
    public bool Select { get; private set; }
    public bool Back { get; private set; }

    public void ReadInput(string actionName, UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        switch (actionName)
        {
            case MainMenuInput.SELECT: // button.
                Select = context.ReadValueAsButton();
                break;
            case MainMenuInput.NAVIGATION: // Vector2.
                Navigation = context.ReadValue<Vector2>();
                break;
            case MainMenuInput.BACK: // button.
                Back = context.ReadValueAsButton();
                break;
            default:
                Debug.LogError($"The action '{actionName}' is not supported.");
                break;
        }
    }
}