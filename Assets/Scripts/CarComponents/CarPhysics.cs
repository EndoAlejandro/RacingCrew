using CustomUtils;
using UnityEngine;

namespace CarComponents
{
    [RequireComponent(typeof(Car))]
    public class CarPhysics : MonoBehaviour
    {
        public Car Car { get; private set; }
        public float AckermannLeftAngle { get; private set; }
        public float AckermannRightAngle { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        public CarStats Stats => Car.Stats;
        private Vector3 FlatVelocity => Rigidbody.velocity.With(y: 0f);

        private void Awake()
        {
            Car = GetComponent<Car>();
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void Start() => Rigidbody.centerOfMass = Vector3.zero;
        private void Update() => AckermannTurning(Car.Input.y);
        private void FixedUpdate() => SpeedControl();

        private void SpeedControl()
        {
            if (FlatVelocity.magnitude > Car.Stats.MaxSpeed)
                Rigidbody.velocity = FlatVelocity.normalized * Car.Stats.MaxSpeed +
                                     Vector3.one * Rigidbody.velocity.y;
        }

        private void AckermannTurning(float turning)
        {
            var normalizedSpeed = FlatVelocity.magnitude / Car.Stats.MaxSpeed;
            var realTurnRadius = Car.Stats.TurnRadius.GetPointInRange(normalizedSpeed);
            switch (turning)
            {
                case > 0f:
                    AckermannLeftAngle =
                        Mathf.Rad2Deg * Mathf.Atan(Stats.WheelBase / (realTurnRadius + Stats.RearTrack / 2)) * turning;
                    AckermannRightAngle =
                        Mathf.Rad2Deg * Mathf.Atan(Stats.WheelBase / (realTurnRadius - Stats.RearTrack / 2)) * turning;
                    break;
                case < 0f:
                    AckermannLeftAngle =
                        Mathf.Rad2Deg * Mathf.Atan(Stats.WheelBase / (realTurnRadius - Stats.RearTrack / 2)) * turning;
                    AckermannRightAngle =
                        Mathf.Rad2Deg * Mathf.Atan(Stats.WheelBase / (realTurnRadius + Stats.RearTrack / 2)) * turning;
                    break;
                default:
                    AckermannLeftAngle = 0f;
                    AckermannRightAngle = 0f;
                    break;
            }
        }
    }
}