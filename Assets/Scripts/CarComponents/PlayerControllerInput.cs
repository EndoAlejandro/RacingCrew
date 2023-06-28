using UnityEngine;
using UnityEngine.InputSystem;

namespace CarComponents
{
    public class PlayerControllerInput : MonoBehaviour, IControllerInput
    {
        public PlayerInput PlayerInput { get; private set; }
        private void Awake() => PlayerInput = GetComponent<PlayerInput>();
        public float Acceleration { get; private set; }
        public float Break { get; private set; }
        public float Turn { get; private set; }
        public void OnAcceleration(InputValue value) => Acceleration = value.Get<float>();
        public void OnBreak(InputValue value) => Break = value.Get<float>();
        public void OnTurning(InputValue value) => Turn = value.Get<float>();
    }
}