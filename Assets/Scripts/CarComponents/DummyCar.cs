using UnityEngine;

namespace CarComponents
{
    public class DummyCar : MonoBehaviour
    {
        [SerializeField] private CarData data;

        private PlayerControllerInput _controllerInput;
        private Car _car;

        private void Awake()
        {
            _controllerInput = GetComponent<PlayerControllerInput>();
            _car = GetComponentInChildren<Car>();
        }

        private void Start()
        {
            // var car = Instantiate(carPrefab, transform.position, transform.rotation);
            /*var model = data.Models[0];
            var racer = new Racer(data, model, _controllerInput);
            _car.Setup(racer);*/
        }
    }
}