using System.Collections;
using System.Collections.Generic;
using CustomUtils;
using RaceComponents;
using Unity.Mathematics;
using UnityEngine;

namespace CarComponents
{
    public class AiCarControllerInput : MonoBehaviour, ICarControllerInput
    {
        private readonly float _refreshRate = 2f;
        public float Acceleration { get; private set; }

        public float Break { get; private set; }
        public float Turn { get; private set; }

        public Vector3 DesiredDirection { get; private set; }

        private int _index;
        private bool _isPlaying;
        private Car _car;
        private Rigidbody _rigidbody;

        private List<float> _deltaPosition = new List<float>();

        private void Awake()
        {
            TrackManager.Instance.OnGo += TrackManagerOnGo;
            TrackManager.Instance.OnRaceOver += TrackManagerOnRaceOver;
            _car = GetComponent<Car>();
        }

        private void TrackManagerOnRaceOver() => _isPlaying = false;

        private void TrackManagerOnGo() => StartCoroutine(RecordDistance());

        private IEnumerator RecordDistance()
        {
            while (_isPlaying)
            {
                var previousPosition = transform.position;
                yield return new WaitForSeconds(_refreshRate);
                var currentPosition = transform.position;

                if (Vector3.Distance(previousPosition, currentPosition) > 1) continue;

                _rigidbody.isKinematic = true;
                var normalizedPosition = NavigationRoute.Instance.GetSplineNormalizedPosition(transform.position);
                NavigationRoute.Instance.EvaluateSpline(normalizedPosition, out float3 position,
                    out float3 direction, out float3 up);
                transform.position = position;
                transform.rotation = Quaternion.LookRotation(direction, up);
                yield return null;
                _rigidbody.isKinematic = false;
            }
        }

        private void Update()
        {
            var target = TrackManager.Instance.GetNextCheckPoint(_car.RacerPosition.LastPointIndex + 1);
            var normalizedPosition =
                NavigationRoute.Instance.GetSplineNormalizedPosition(target.transform.position);
            NavigationRoute.Instance.EvaluateSpline(normalizedPosition, out float3 position, out float3 direction,
                out float3 up);

            DesiredDirection = (((Vector3)position).With(y: 0f) - _car.transform.position.With(y: 0f)).normalized;

            var dot = Vector3.Dot(_car.transform.forward, DesiredDirection);
            var angle =
                Vector3.SignedAngle(DesiredDirection, _car.transform.forward, Vector3.up) * -0.45f;

            if (dot > 0.25f)
            {
                Acceleration = Mathf.Abs(angle) < 60f ? dot : 0;
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
}