using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField] private float wheelRadius = 0.6f;
        [SerializeField] private Transform display;
        [SerializeField] private WheelPosition wheelPosition;

        private VehiclePhysics _vehiclePhysics;

        // Visuals.
        private Vector3 _initialPosition;
        private Vector3 _targetPosition;

        // Physics.
        private Vector3 _suspensionForce;
        private Vector3 _steeringForce;
        private Vector3 _accelerationForce;

        private RaycastHit _hit;

        private void Awake()
        {
            _initialPosition = display.localPosition;
            _targetPosition = _initialPosition;
            _vehiclePhysics = GetComponentInParent<VehiclePhysics>();
            _hit = new RaycastHit();
        }

        private void Update()
        {
            WheelRotationDisplay();
            WheelPositionSmooth();
        }

        private void WheelPositionSmooth() => display.localPosition =
            Vector3.Lerp(display.localPosition, _targetPosition, Time.deltaTime * 10f);

        private void WheelRotationDisplay()
        {
            transform.localRotation = wheelPosition switch
            {
                WheelPosition.FrontLeft => Quaternion.Euler(Vector3.up * _vehiclePhysics.AckermannLeftAngle),
                WheelPosition.FrontRight => Quaternion.Euler(Vector3.up * _vehiclePhysics.AckermannRightAngle),
                _ => transform.localRotation
            };
        }

        private void FixedUpdate()
        {
            if (!Physics.Raycast(transform.position, -_vehiclePhysics.transform.up * wheelRadius, out _hit,
                    _vehiclePhysics.SuspensionDistance + wheelRadius))
            {
                _targetPosition = _initialPosition + Vector3.up * (wheelRadius - _vehiclePhysics.SuspensionDistance);
                GravityForce();
                return;
            }

            _targetPosition = _hit.point - transform.position + _hit.normal * wheelRadius + _initialPosition;

            var worldVelocity = _vehiclePhysics.Rigidbody.GetPointVelocity(transform.position);
            Grip(worldVelocity);
            Suspension(worldVelocity);
            Acceleration(worldVelocity);
        }

        private void GravityForce()
        {
            _vehiclePhysics.Rigidbody.AddForce(Vector3.down * 10f, ForceMode.Acceleration);
        }

        /// <summary>
        /// Acceleration force made by the wheel but it's applied in the vehicle's Rigidbody.
        /// </summary>
        /// <param name="worldVelocity"></param>
        private void Acceleration(Vector3 worldVelocity)
        {
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