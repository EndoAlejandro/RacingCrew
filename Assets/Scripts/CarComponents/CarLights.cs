using UnityEngine;

namespace CarComponents
{
    public class CarLights : CarComponent
    {
        [SerializeField] private GameObject backLights;
        private void Update() => backLights.SetActive(car.Input.z > 0);
    }
}