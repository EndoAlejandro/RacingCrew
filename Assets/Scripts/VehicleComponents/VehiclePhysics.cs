using System;
using UnityEngine;

namespace VehicleComponents
{
    [RequireComponent(typeof(Rigidbody))]
    public class VehiclePhysics : MonoBehaviour
    {
        [Header("Vehicle")]
        [SerializeField] private float speed = 1f;

        [SerializeField] private float steering = 1f;

        [Header("Suspension")]
        [SerializeField] private float suspensionDistance = 2f;

        [SerializeField] private float springStrength = 100f;
        [SerializeField] private float damp = 1f;

        [Header("Steering")]
        [Range(0f, 60f)]
        [SerializeField] private float steerAngle = 45f;

        [SerializeField] private float tireGrip;

        [SerializeField] private float tireMass;

        [Header("Acceleration")]
        [SerializeField] private float acceleration;

        [SerializeField] private float maxSpeed;

        [SerializeField] private AnimationCurve accelerationCurve;


        public float SuspensionDistance => suspensionDistance;
        public float SpringStrength => springStrength;
        public float Damp => damp;
        public float TireGrip => tireGrip;
        public float TireMass => tireMass;
        public float MaxSpeed => maxSpeed;
        public float Acceleration => acceleration;
        public float SteerAngle => steerAngle;
        public AnimationCurve AccelerationCurve => accelerationCurve;
        public Rigidbody Rigidbody { get; private set; }

        private void Awake() => Rigidbody = GetComponent<Rigidbody>();

        private void FixedUpdate()
        {
            /*Rigidbody.AddForceAtPosition(new Vector3(horizontal, 0f, vertical),
                transform.position + transform.forward * -2f);*/
        }
    }
}