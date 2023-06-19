using CarComponents;
using UnityEngine;

namespace RaceComponents
{
    public class CheckPoint : MonoBehaviour
    {
        [SerializeField] private bool isMandatory;
        public bool IsMandatory => isMandatory;
        private TrackCheckPoints _trackCheckPoint;

        public void Setup(TrackCheckPoints trackCheckPoints) => _trackCheckPoint = trackCheckPoints;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Car car))
                _trackCheckPoint.CarThroughCheckPoint(this, car);
        }
    }
}