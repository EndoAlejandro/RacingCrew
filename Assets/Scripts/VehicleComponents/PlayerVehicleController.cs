using UnityEngine;
using UnityEngine.InputSystem;

namespace VehicleComponents
{
    public class PlayerVehicleController : MonoBehaviour
    {
        [SerializeField] private VehiclePhysics vehiclePrefab;

        private VehiclePhysics _spawnedVehicle;

        private void Start() => _spawnedVehicle = Instantiate(vehiclePrefab);

        public void AccelerationInput(InputAction.CallbackContext context) =>
            _spawnedVehicle.Accelerate(context.ReadValue<float>());

        public void BreakInput(InputAction.CallbackContext context) =>
            _spawnedVehicle.Accelerate(-context.ReadValue<float>());

        public void TurningInput(InputAction.CallbackContext context) =>
            _spawnedVehicle.Turn(context.ReadValue<float>());
    }
}