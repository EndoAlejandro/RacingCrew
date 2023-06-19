using RaceComponents;
using UnityEngine;
using VehicleComponents;

namespace CarComponents
{
    public class DummyCar : MonoBehaviour
    {
        [SerializeField] private CarData data;
        // [SerializeField] private Car carPrefab;

        private PlayerControllerInput _controllerInput;
        private Car _car;

        private void Awake()
        {
            _controllerInput = GetComponent<PlayerControllerInput>();
            _car = GetComponentInChildren<Car>();
        }

        private void Start()
        {
            var model = data.Models[0];
            var racer = new Racer(data, model, _controllerInput);
            _car.Setup(racer);
        }
    }
}