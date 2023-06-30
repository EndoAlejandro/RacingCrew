using System.Collections;
using System.Collections.Generic;
using CustomUtils;
using InputManagement;
using RaceComponents;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace CarComponents
{
    public class AiCarControllerInput : CarComponent, ICarControllerInput
    {
        private readonly float _refreshRate = 2f;
        public float Acceleration { get; private set; }

        public float Break { get; private set; }
        public float Turn { get; private set; }

        public Vector3 DesiredDirection { get; private set; }

        private int _index;
        private bool _isPlaying;
        private Rigidbody _rigidbody;

        private List<float> _deltaPosition = new List<float>();

        protected override void Awake()
        {
            base.Awake();
            TrackManager.Instance.OnGo += TrackManagerOnGo;
            TrackManager.Instance.OnRaceOver += TrackManagerOnRaceOver;
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
            var t = transform.position;
            var target = TrackManager.Instance.GetNextCheckPoint(car.RacerPosition.LastPointIndex - 1);
            var nextTarget = TrackManager.Instance.GetNextCheckPoint(car.RacerPosition.LastPointIndex + 1);

            var normalizedTargetPosition =
                NavigationRoute.Instance.GetSplineNormalizedPosition(target.transform.position);
            NavigationRoute.Instance.EvaluateSpline(normalizedTargetPosition,
                out float3 targetPosition,
                out float3 targetDirection,
                out float3 targetUp);

            NavigationRoute.Instance.GetSplineNormalizedPosition(nextTarget.transform.position);
            NavigationRoute.Instance.EvaluateSpline(normalizedTargetPosition,
                out float3 nextTargetPosition,
                out float3 nextTargetDirection,
                out float3 nextTargetUp);

            var normalizedClosestPosition = NavigationRoute.Instance.GetSplineNormalizedPosition(t);

            var targetPointDirection = Utils.NormalizedFlatDirection(nextTarget.transform.position, t);
            // DesiredDirection = (((Vector3)position).With(y:0) - transform.position).normalized;

            DesiredDirection = /*distanceToClosestPoint < 3f ? closestPointDirection :*/ targetPointDirection;

            var targetDot = Vector3.Dot(transform.forward, targetPointDirection);
            var pointsDot = 1 - Mathf.Abs(Vector3.Dot(((Vector3)targetDirection).normalized,
                ((Vector3)nextTargetDirection).normalized));
            var angle = Vector3.SignedAngle(transform.forward, targetPointDirection, Vector3.up) * 0.5f;

            var multiplier = 1;
            var averageDot = (targetDot + pointsDot) / 2;

            if (targetDot > 0.2f)
            {
                Acceleration = targetDot - pointsDot;
                Break = pointsDot;
            }
            else
            {
                Acceleration = 0f;
                Break = 1;
                multiplier = -1;
            }

            var top = 25f;
            var clamped = Mathf.Clamp(angle, -top, top);
            var normalized = clamped / top;
            //Turn = Mathf.Clamp(angle * multiplier, -1, 1);
            Turn = normalized * multiplier;
        }

        private void Legacy()
        {
            var target = TrackManager.Instance.GetNextCheckPoint(car.RacerPosition.LastPointIndex + 1);
            var normalizedPosition =
                NavigationRoute.Instance.GetSplineNormalizedPosition(target.transform.position);
            NavigationRoute.Instance.EvaluateSpline(normalizedPosition, out float3 position, out float3 direction,
                out float3 up);

            DesiredDirection = Utils.NormalizedFlatDirection(position, transform.position);
            // DesiredDirection = (((Vector3)position).With(y:0) - transform.position).normalized;

            var dot = Vector3.Dot(transform.forward, DesiredDirection);
            var angle = Vector3.SignedAngle(DesiredDirection, transform.forward, Vector3.up);

            if (dot > 0.25f)
            {
                Acceleration = dot;
                Break = 0f;
                angle *= -1;
            }
            else
            {
                Acceleration = 0f;
                Break = 1f;
            }

            if (Mathf.Abs(angle) > 5f)
                Turn = Mathf.Clamp(angle, -1, 1);
        }
    }
}