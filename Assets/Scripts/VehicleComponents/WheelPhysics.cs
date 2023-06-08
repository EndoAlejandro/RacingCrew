using System;
using UnityEngine;

namespace VehicleComponents
{
    public enum WheelPosition
    {
        FrontLeft,
        FrontRight,
        RearLeft,
        RearRight,
    }

    public class WheelPhysics : MonoBehaviour
    {
        [SerializeField] private WheelPosition wheelPosition;

        private VehiclePhysics _vehiclePhysics;

        private Vector3 _suspensionForce;
        private Vector3 _steeringForce;
        private Vector3 _accelerationForce;

        private RaycastHit _hit;

        private void Awake()
        {
            _vehiclePhysics = GetComponentInParent<VehiclePhysics>();
            _hit = new RaycastHit();
        }

        private void Update()
        {
            switch (wheelPosition)
            {
                case WheelPosition.FrontLeft:
                    transform.localRotation = Quaternion.Euler(Vector3.up * _vehiclePhysics.AckermannLeftAngle);
                    break;
                case WheelPosition.FrontRight:
                    transform.localRotation = Quaternion.Euler(Vector3.up * _vehiclePhysics.AckermannRightAngle);
                    break;
            }
        }

        private void FixedUpdate()
        {
            if (!Physics.Raycast(transform.position, -_vehiclePhysics.transform.up, out _hit,
                    _vehiclePhysics.SuspensionDistance)) return;

            var worldVelocity = _vehiclePhysics.Rigidbody.GetPointVelocity(transform.position);
            Grip(worldVelocity);
            Suspension(worldVelocity);
            Acceleration();
        }

        private void Acceleration()
        {
            if (wheelPosition.ToString().Contains("Front")) return;

            var vertical = Input.GetAxis("Vertical");

            var accelerationDirection = transform.forward;
            var speed = Vector3.Dot(_vehiclePhysics.transform.forward, _vehiclePhysics.Rigidbody.velocity);
            var normalizedSpeed = Mathf.Clamp01(Mathf.Abs(speed) / _vehiclePhysics.MaxSpeed);

            var torque = _vehiclePhysics.AccelerationCurve.Evaluate(normalizedSpeed) * vertical;
            _accelerationForce = accelerationDirection * (torque * _vehiclePhysics.Acceleration);

            var positionForce = transform.position;
            positionForce.y = _vehiclePhysics.transform.position.y + 0f;
            _vehiclePhysics.Rigidbody.AddForceAtPosition(_accelerationForce, positionForce, ForceMode.Acceleration);
        }

        private void Grip(Vector3 worldVelocity)
        {
            var steeringDirection = transform.right;
            var steeringVelocity = Vector3.Dot(steeringDirection, worldVelocity);

            var desiredVelocityChange = -steeringVelocity * _vehiclePhysics.CurrentGrip;
            var desiredAcceleration = desiredVelocityChange / Time.fixedDeltaTime;

            _steeringForce = steeringDirection * _vehiclePhysics.TireMass * desiredAcceleration;
            _vehiclePhysics.Rigidbody.AddForceAtPosition(_steeringForce, transform.position, ForceMode.Acceleration);
        }

        private void Suspension(Vector3 worldVelocity)
        {
            var springDirection = transform.up;
            var offset = _vehiclePhysics.SuspensionDistance - _hit.distance;

            var velocity = Vector3.Dot(springDirection, worldVelocity);
            var force = (offset * _vehiclePhysics.SpringStrength) - (velocity * _vehiclePhysics.Damp);

            _suspensionForce = springDirection * force;
            _vehiclePhysics.Rigidbody.AddForceAtPosition(_suspensionForce, transform.position, ForceMode.Acceleration);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + _suspensionForce);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + _steeringForce);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + _accelerationForce);
        }
    }
}