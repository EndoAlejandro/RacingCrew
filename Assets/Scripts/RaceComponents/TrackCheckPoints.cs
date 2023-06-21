using CarComponents;
using UnityEngine;

namespace RaceComponents
{
    public class TrackCheckPoints : MonoBehaviour
    {
        private CheckPoint[] _checkPoints;

        private void Awake()
        {
            _checkPoints = transform.GetComponentsInChildren<CheckPoint>();
            foreach (var checkPoint in _checkPoints) checkPoint.Setup(this);
        }

        public void CarThroughCheckPoint(CheckPoint checkPoint, Car component)
        {
            
        }
    }
}