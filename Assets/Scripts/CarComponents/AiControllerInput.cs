using CustomUtils;
using RaceComponents;
using UnityEngine;

namespace CarComponents
{
    public class AiControllerInput : MonoBehaviour, IControllerInput, IDirectionNavigator
    {
        public float Acceleration { get; private set; }

        public float Break { get; private set; }
        public float Turn { get; private set; }

        public Vector3 DesiredDirection { get; private set; }

        private int _index;
        private Car _car;

        public void Setup(int index) => _index = index;

        private void Update()
        {
            if (_car == null && TrackManager.Instance.Cars.Count > _index)
            {
                _car = TrackManager.Instance.Cars[_index];
            }
            else
            {
                var target = TrackManager.Instance.GetNextCheckPoint(_car.Racer.RacerPosition.LastPointIndex);
                if (Vector3.Distance(target.transform.position.With(y: 0f), _car.transform.position.With(y: 0f)) < 20f)
                    target = TrackManager.Instance.GetNextCheckPoint(_car.Racer.RacerPosition.LastPointIndex + 1);
                DesiredDirection = (target.transform.position.With(y: 0f) - _car.transform.position.With(y: 0f))
                    .normalized;
            }

            var dot = Vector3.Dot(_car.transform.forward, DesiredDirection);
            var angle =
                Vector3.SignedAngle(DesiredDirection, _car.transform.forward, Vector3.up) * -0.45f;

            if (dot > 0 && Mathf.Abs(angle) < 60f)
            {
                Acceleration = dot;
                Break = 0f;
            }
            else
            {
                Acceleration = 0f;
                Break = 1f;
                angle *= -1;
            }

            //if (Mathf.Abs(angle) > 15f)
                Turn = Mathf.Clamp(angle, -1, 1);
        }
    }

    public interface IDirectionNavigator
    {
        Vector3 DesiredDirection { get; }
        void Setup(int index);
    }
}