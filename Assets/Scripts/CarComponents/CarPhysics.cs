using System;
using CustomUtils;
using UnityEngine;

namespace CarComponents
{
    [RequireComponent(typeof(Car))]
    public class CarPhysics : MonoBehaviour
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

        public Car Car { get; private set; }

        public float SuspensionDistance => suspensionDistance;
        public float SpringStrength => springStrength;
        public float Damp => damp;
        public float CurrentGrip => tireGrip;
        public float TireMass => tireMass;
        public float MaxSpeed => maxSpeed;
        public float Acceleration => acceleration;
        public float AccelerationInput { get; private set; }
        public float AckermannLeftAngle { get; private set; }
        public float AckermannRightAngle { get; private set; }
        public AnimationCurve AccelerationCurve => accelerationCurve;
        public Rigidbody Rigidbody { get; private set; }
        private Vector3 FlatVelocity => Rigidbody.velocity.With(y: 0f);

        private void Awake()
        {
            Car = GetComponent<Car>();
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void Start() => Rigidbody.centerOfMass = Vector3.zero;

        private void Update()
        {
            var turningRaw = Car.Input.y;
            var normalizedSpeed = FlatVelocity.magnitude / Car.Data.MaxSpeed;
            var radius = Mathf.Lerp(Car.Data.TurnRadius.x, Car.Data.TurnRadius.y,
                FlatVelocity.magnitude / Car.Data.MaxSpeed);
            AckermannTurning(turningRaw * normalizedSpeed);
        }

        private void FixedUpdate() => SpeedControl();

        private void SpeedControl()
        {
            if (FlatVelocity.magnitude > Car.Data.MaxSpeed)
                Rigidbody.velocity = FlatVelocity.normalized * Car.Data.MaxSpeed +
                                     Vector3.one * Rigidbody.velocity.y;
        }

        private void AckermannTurning(float turning)
        {
            switch (turning)
            {
                case > 0f:
                    AckermannLeftAngle =
                        Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + rearTrack / 2)) * turning;
                    AckermannRightAngle =
                        Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - rearTrack / 2)) * turning;
                    break;
                case < 0f:
                    AckermannLeftAngle =
                        Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - rearTrack / 2)) * turning;
                    AckermannRightAngle =
                        Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + rearTrack / 2)) * turning;
                    break;
                default:
                    AckermannLeftAngle = 0f;
                    AckermannRightAngle = 0f;
                    break;
            }
        }
    }
}