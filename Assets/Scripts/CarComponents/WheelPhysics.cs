using CarComponents;
using UnityEngine;

namespace VehicleComponents
{
    public class WheelPhysics : MonoBehaviour
    {
        [SerializeField] private float wheelRadius = 0.6f;
        [SerializeField] private Transform display;
        [SerializeField] private WheelPosition wheelPosition;

        private CarPhysics _carPhysics;

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
            _carPhysics = GetComponentInParent<CarPhysics>();
            _hit = new RaycastHit();
        }

        private void Update()
        {
            WheelRotationDisplay();
            WheelPositionSmooth();
        }

        /// <summary>
        /// Controls the vertical position of the wheels to simulate the wheel position working with the suspension.
        /// </summary>
        private void WheelPositionSmooth() => display.localPosition =
            Vector3.Lerp(display.localPosition, _targetPosition, Time.deltaTime * 10f);

        /// <summary>
        /// Rotate the wheel based on the ackermann calculation made from the vehicle physics.
        /// </summary>
        private void WheelRotationDisplay()
        {
            transform.localRotation = wheelPosition switch
            {
                WheelPosition.FrontLeft => Quaternion.Euler(Vector3.up * _carPhysics.AckermannLeftAngle),
                WheelPosition.FrontRight => Quaternion.Euler(Vector3.up * _carPhysics.AckermannRightAngle),
                _ => transform.localRotation
            };
        }

        private void FixedUpdate()
        {
            if (!Physics.Raycast(transform.position + transform.up * 0.25f, -_carPhysics.transform.up * wheelRadius,
                    out _hit,
                    _carPhysics.SuspensionDistance + wheelRadius))
            {
                _targetPosition = _initialPosition + Vector3.up * (wheelRadius - _carPhysics.SuspensionDistance);
                GravityForce();
                return;
            }

            _targetPosition = _hit.point - transform.position + _hit.normal * wheelRadius + _initialPosition;

            var worldVelocity = _carPhysics.Rigidbody.GetPointVelocity(transform.position);
            Grip(worldVelocity);
            Suspension(worldVelocity);
            Acceleration(worldVelocity);
        }

        /// <summary>
        /// Make the vehicle fall faster. kind of extra gravity.
        /// </summary>
        private void GravityForce() => _carPhysics.Rigidbody.AddForce(Vector3.down * 10f, ForceMode.Acceleration);

        /// <summary>
        /// Acceleration force made by the wheel but it's applied in the vehicle's Rigidbody.
        /// </summary>
        /// <param name="worldVelocity"></param>
        private void Acceleration(Vector3 worldVelocity)
        {
            var vertical = _carPhysics.Car.Input.x - _carPhysics.Car.Input.z;

            var accelerationDirection = transform.forward;
            var speed = Vector3.Dot(_carPhysics.transform.forward, _carPhysics.Rigidbody.velocity);
            var normalizedSpeed = Mathf.Clamp01(Mathf.Abs(speed) / _carPhysics.MaxSpeed);

            var torque = _carPhysics.AccelerationCurve.Evaluate(normalizedSpeed) * vertical;
            _accelerationForce = accelerationDirection * (torque * _carPhysics.Acceleration);

            var positionForce = transform.position;
            positionForce.y = _carPhysics.transform.position.y + 0f;
            _carPhysics.Rigidbody.AddForceAtPosition(_accelerationForce, positionForce, ForceMode.Acceleration);
        }

        /// <summary>
        /// Grip force of the tire to avoid drifting.
        /// </summary>
        /// <param name="worldVelocity"></param>
        private void Grip(Vector3 worldVelocity)
        {
            var steeringDirection = transform.right;
            var steeringVelocity = Vector3.Dot(steeringDirection, worldVelocity);

            var desiredVelocityChange = -steeringVelocity * _carPhysics.CurrentGrip;
            var desiredAcceleration = desiredVelocityChange / Time.fixedDeltaTime;

            _steeringForce = steeringDirection * _carPhysics.TireMass * desiredAcceleration;
            _carPhysics.Rigidbody.AddForceAtPosition(_steeringForce, transform.position, ForceMode.Acceleration);
        }

        /// <summary>
        /// How the spring of the wheel should work.
        /// </summary>
        /// <param name="worldVelocity"></param>
        private void Suspension(Vector3 worldVelocity)
        {
            var springDirection = transform.up;
            var offset = _carPhysics.SuspensionDistance - _hit.distance;

            var velocity = Vector3.Dot(springDirection, worldVelocity);
            var force = (offset * _carPhysics.SpringStrength) - (velocity * _carPhysics.Damp);

            _suspensionForce = springDirection * force;
            _carPhysics.Rigidbody.AddForceAtPosition(_suspensionForce, transform.position, ForceMode.Acceleration);
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