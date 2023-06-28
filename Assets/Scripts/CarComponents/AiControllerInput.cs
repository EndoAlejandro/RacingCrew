using System.Collections;
using System.Collections.Generic;
using CustomUtils;
using RaceComponents;
using Unity.Mathematics;
using UnityEngine;

namespace CarComponents
{
    public class AiControllerInput : MonoBehaviour, IControllerInput, IDirectionNavigator
    {
        [SerializeField] private float refreshRate;
        public float Acceleration { get; private set; }

        public float Break { get; private set; }
        public float Turn { get; private set; }

        public Vector3 DesiredDirection { get; private set; }

        private int _index;
        private bool _isPlaying;
        private Car _car;

        private List<float> _deltaPosition = new List<float>();

        public void Setup(int index)
        {
            TrackManager.Instance.OnGo += TrackManagerOnGo;
            TrackManager.Instance.OnRaceOver += TrackManagerOnRaceOver;
            _index = index;
        }

        private void TrackManagerOnRaceOver() => _isPlaying = false;

        private void TrackManagerOnGo() => StartCoroutine(RecordDistance());

        private IEnumerator RecordDistance()
        {
            while (_isPlaying)
            {
                yield return new WaitForSeconds(refreshRate);
            }
        }

        private void Update()
        {
            if (_car == null && TrackManager.Instance.Cars.Count > _index)
            {
                _car = TrackManager.Instance.Cars[_index];
            }
            else
            {
                var target = TrackManager.Instance.GetNextCheckPoint(_car.Racer.RacerPosition.LastPointIndex);
                var normalizedPosition =
                    NavigationRoute.Instance.GetSplineNormalizedPosition(target.transform.position);
                NavigationRoute.Instance.EvaluateSpline(normalizedPosition, out float3 position, out float3 direction,
                    out float3 up);

                DesiredDirection = (((Vector3)position).With(y: 0f) - _car.transform.position.With(y: 0f)).normalized;
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

            Turn = Mathf.Clamp(angle, -1, 1);
        }
    }

    public interface IDirectionNavigator
    {
        Vector3 DesiredDirection { get; }
        void Setup(int index);
    }
}