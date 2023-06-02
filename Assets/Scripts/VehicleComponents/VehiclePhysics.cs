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
        
        // [Header("Steering")]
        // [SerializeField] private float 

        public float SuspensionDistance => suspensionDistance;
        public float SpringStrength => springStrength;
        public float Damp => damp;
        public Rigidbody Rigidbody { get; private set; }
        private void Awake() => Rigidbody = GetComponent<Rigidbody>();

        private void FixedUpdate()
        {
            var vertical = Input.GetAxisRaw("Vertical") * speed;
            var horizontal = Input.GetAxisRaw("Horizontal") * steering;

            /*Rigidbody.AddForceAtPosition(new Vector3(horizontal, 0f, vertical),
                transform.position + transform.forward * -2f);*/
        }
    }
}