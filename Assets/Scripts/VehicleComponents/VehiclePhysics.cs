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
        [SerializeField] private float tireGrip;

        [SerializeField] private float tireMass;

        [Header("Acceleration")]
        [SerializeField] private AnimationCurve accelerationCurve;

        [SerializeField] private float acceleration;
        [SerializeField] private float maxSpeed;

        [Header("Ackermann Steering")]
        [SerializeField] private float wheelBase;

        [SerializeField] private float rearTrack;
        [SerializeField] private float turnRadius;

        public float SuspensionDistance => suspensionDistance;
        public float SpringStrength => springStrength;
        public float Damp => damp;
        public float CurrentGrip { get; private set; }
        public float TireMass => tireMass;
        public float MaxSpeed => maxSpeed;
        public float Acceleration => acceleration;

        public AnimationCurve AccelerationCurve => accelerationCurve;
        public Rigidbody Rigidbody { get; private set; }

        public float AckermannLeftAngle { get; private set; }
        public float AckermannRightAngle { get; private set; }

        private void Awake() => Rigidbody = GetComponent<Rigidbody>();
        private void Start() => Rigidbody.centerOfMass = Vector3.zero;

        private void Update()
        {
            var horizontal = Input.GetAxis("Horizontal");

            var targetGrip = tireGrip;
            if (horizontal != 0f && Input.GetKey(KeyCode.LeftShift)) targetGrip = 0f;

            CurrentGrip = Mathf.Lerp(CurrentGrip, targetGrip, Time.deltaTime * 10f);

            switch (horizontal)
            {
                case > 0f:
                    AckermannLeftAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) *
                                         horizontal;
                    AckermannRightAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) *
                                          horizontal;

                    Rigidbody.centerOfMass = Vector3.left;
                    break;
                case < 0f:
                    AckermannLeftAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - rearTrack / 2)) *
                                         horizontal;
                    AckermannRightAngle = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) *
                                          horizontal;
                    Rigidbody.centerOfMass = Vector3.right;
                    break;
                default:
                    Rigidbody.centerOfMass = Vector3.zero;
                    AckermannLeftAngle = 0f;
                    AckermannRightAngle = 0f;
                    break;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log(other.transform.name);
        }
    }
}