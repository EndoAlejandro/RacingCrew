using UnityEngine;

namespace CarComponents
{
    [RequireComponent(typeof(Car))]
    public abstract class CarComponent : MonoBehaviour
    {
        protected Car car;
        protected virtual void Awake() => car = GetComponent<Car>();
    }
}