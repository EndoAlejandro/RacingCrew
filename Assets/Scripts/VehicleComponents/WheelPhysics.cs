using UnityEngine;

namespace VehicleComponents
{
    public class WheelPhysics : MonoBehaviour
    {
        [SerializeField] private bool canSteer;

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
            if (!canSteer) return;

            var horizontal = Input.GetAxis("Horizontal");
            var angle = _vehiclePhysics.SteerAngle * horizontal;
            var desired = Quaternion.Euler(new Vector3(0f, angle, 0f));
            var lerp = Quaternion.Lerp(transform.localRotation, desired, Time.deltaTime * _vehiclePhysics.SteerForce);
            transform.localRotation = lerp;
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
            var vertical = Input.GetAxis("Vertical");

            // if (!(vertical > 0f)) return;
            var accelerationDirection = transform.forward;
            var speed = Vector3.Dot(_vehiclePhysics.transform.forward, _vehiclePhysics.Rigidbody.velocity);
            var normalizedSpeed = Mathf.Clamp01(Mathf.Abs(speed) / _vehiclePhysics.MaxSpeed);

            var torque = _vehiclePhysics.AccelerationCurve.Evaluate(normalizedSpeed) * vertical;
            _accelerationForce = accelerationDirection * (torque * _vehiclePhysics.Acceleration);

            _vehiclePhysics.Rigidbody.AddForceAtPosition(_accelerationForce, transform.position,
                ForceMode.Acceleration);
        }

        private void Grip(Vector3 worldVelocity)
        {
            var steeringDirection = transform.right;
            var steeringVelocity = Vector3.Dot(steeringDirection, worldVelocity);

            var desiredVelocityChange = -steeringVelocity * _vehiclePhysics.TireGrip;
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