using CarComponents;
using UnityEngine;

namespace RaceComponents
{
    public class CheckPoint : MonoBehaviour
    {
        [SerializeField] private bool isMandatory;
        public bool IsMandatory => isMandatory;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out Car car)) return;
            var dot = Vector3.Dot(car.transform.forward, transform.forward);
            if (dot > 0) TrackManager.Instance.CarThroughCheckPoint(this, car);
        }
    }
}