using System;
using UnityEngine;

namespace VehicleComponents
{
    [RequireComponent(typeof(Rigidbody))]
    public class VehiclePhysics : MonoBehaviour
    {
        [Header("Vehicle")]
        [Header("Suspension")]
        [SerializeField] private float suspensionDistance = 2f;

        [SerializeField] private float springStrength = 100f;
        [SerializeField] private float damp = 1f;

        [Header("Steering")]
        [SerializeField]
        [Range(0f, 60f)] private float steeringAngle;

        [SerializeField] private float steerForce = 1f;
        [SerializeField] private float tireGrip;
        [SerializeField] private float tireMass;

        [Header("Acceleration")]
        [SerializeField] private AnimationCurve accelerationCurve;

        [SerializeField] private float acceleration;
        [SerializeField] private float maxSpeed;

        public float SuspensionDistance => suspensionDistance;
        public float SpringStrength => springStrength;
        public float Damp => damp;
        public float TireGrip => tireGrip;
        public float TireMass => tireMass;
        public float MaxSpeed => maxSpeed;
        public float Acceleration => acceleration;
        public float SteeringAngle => steeringAngle;
        public float SteerForce => steerForce;
        public AnimationCurve AccelerationCurve => accelerationCurve;
        public Rigidbody Rigidbody { get; private set; }

        private void Awake() => Rigidbody = GetComponent<Rigidbody>();

        private void Start()
        {
            // Rigidbody.centerOfMass = Vector3.up * 0f;
        }

        private void Update()
        {
            // Rigidbody.AddRelativeTorque(transform.up * (Input.GetAxis("Horizontal") * SteerForce));
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log(other.transform.name);
        }
    }
}