using System;
using UnityEngine;

namespace CarComponents
{
    [Serializable]
    public class CarStats
    {
        [Header("Acceleration")]
        [Tooltip("Max possible horizontal speed.")]
        [SerializeField] private float maxSpeed = 200f;

        [Tooltip("Acceleration force based on speed.")]
        [SerializeField] private float horsePower = 300f;

        [Range(0.001f, 1f)] [SerializeField] private float reverseAccelerationScale = 0.75f;
        [SerializeField] private float breakForce;

        [Tooltip("This emulate the force of the engine.")]
        [SerializeField] private AnimationCurve accelerationCurve;

        [Header("Turning")]
        [Tooltip("Friction of the wheels with the floor.")]
        [Range(0f, 1f)] [SerializeField] private float grip = 0.5f;

        [Tooltip("TODO: Tire Mass.")]
        [SerializeField] private float tireMass = 0.2f;

        [Tooltip("Turn radius ranged. x = turn radius at 0Km/h | y = Turn Radius at MaxSpeed.")]
        [SerializeField] private Vector2 turnRadius;

        [Tooltip("Distance from left wheels to right wheels.")]
        [SerializeField] private float rearTrack = 2.5f;

        [Tooltip("Distance from rear wheels to front wheels.")]
        [SerializeField] private float wheelBase = 2.629f;

        [Header("Suspension")]
        [Tooltip("Force of each wheel spring.")]
        [SerializeField] private float suspensionForce;

        [Tooltip("Lenght of the spring.")]
        [SerializeField] private float suspensionDistance = 2f;

        [Tooltip("Reduce de buoyancy of the spring.")]
        [SerializeField] private float damp = 5f;

        public float MaxSpeed => maxSpeed;
        public float HorsePower => horsePower;
        public float ReverseAccelerationScale => reverseAccelerationScale;
        public float BreakForce => breakForce;
        public AnimationCurve AccelerationCurve => accelerationCurve;
        public float Grip => grip;
        public float TireMass => tireMass;
        public Vector2 TurnRadius => turnRadius;
        public float RearTrack => rearTrack;
        public float WheelBase => wheelBase;
        public float SuspensionForce => suspensionForce;
        public float SuspensionDistance => suspensionDistance;
        public float Damp => damp;
    }
}