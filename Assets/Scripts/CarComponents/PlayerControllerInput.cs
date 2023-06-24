using UnityEngine;
using UnityEngine.InputSystem;

namespace CarComponents
{
    public class PlayerControllerInput : MonoBehaviour, IControllerInput
    {
        private PlayerInput _playerInput;
        private void Awake() => _playerInput = GetComponent<PlayerInput>();
        public float Acceleration { get; private set; }
        public float Break { get; private set; }
        public float Turn { get; private set; }
        public bool SwichtCamera { get; private set; }
        public void OnAcceleration(InputValue value) => Acceleration = value.Get<float>();
        public void OnBreak(InputValue value) => Break = value.Get<float>();
        public void OnTurning(InputValue value) => Turn = value.Get<float>();
        // public void OnCameraSwitch
    }
}