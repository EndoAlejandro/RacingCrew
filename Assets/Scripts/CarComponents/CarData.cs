using UnityEngine;

namespace CarComponents
{
    [CreateAssetMenu(menuName = "Scriptable Objects/New Car Data", fileName = "NewCarData", order = 3)]
    public class CarData : ScriptableObject
    {
        [SerializeField] private float maxSpeed = 200f;
        [SerializeField] private float acceleration = 40f;
        [Range(0.001f, 1f)] [SerializeField] private float reverseAccelerationScale = 0.75f;
        [Range(0f, 1f)] [SerializeField] private float grip = 0.5f;
        [SerializeField] private float breakForce;
        [SerializeField] private Vector2 turnRadius;
        [SerializeField] private float suspensionForce;
        [SerializeField] private GameObject[] models;

        public float MaxSpeed => maxSpeed;
        public float Acceleration => acceleration;
        public float ReverseAccelerationScale => reverseAccelerationScale;
        public float Grip => grip;
        public float BreakForce => breakForce;
        public Vector2 TurnRadius => turnRadius;
        public float SuspensionForce => suspensionForce;
        public GameObject[] Models => models;
    }
}