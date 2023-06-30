using UnityEngine;

namespace CarComponents
{
    public class CarLights : MonoBehaviour
    {
        [SerializeField] private GameObject backLights;
        private Car _car;
        private void Awake() => _car = GetComponent<Car>();
        private void Update() => backLights.SetActive(_car.Input.z > 0);
    }
}