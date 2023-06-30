using CustomUtils;
using UnityEngine;

public class MainMenuInputReader : IInputReader
{
    public int Navigation { get; private set; }
    public bool Select { get; private set; }
    public bool Back { get; private set; }

    public void ReadInput(string actionName, UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        switch (actionName)
        {
            case MainMenuInput.SELECT: // button.
                Select = context.action.WasPerformedThisFrame();
                break;
            case MainMenuInput.NAVIGATION: // Vector2.
                Navigation = Mathf.RoundToInt(context.ReadValue<float>());
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