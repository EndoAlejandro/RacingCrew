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
            if (other.TryGetComponent(out Car car))
                TrackManager.Instance.CarThroughCheckPoint(this, car);
        }
    }
}