using System;
using UnityEngine;

namespace VehicleComponents
{
    public class VehicleSphere : MonoBehaviour
    {
        [SerializeField] private Transform display;
        [SerializeField] private Rigidbody sphere;
        [SerializeField] private float acceleration = 1f;
        [SerializeField] private float steering;
        [SerializeField] private float gravity = 9.8f;

        private float _speed;
        private float _rotate;
        private float _currentSpeed;
        private float _currentRotate;

        private void Update()
        {
            display.position = sphere.transform.position; //- Vector3.up * 0.2f;

            var vertical = Input.GetAxisRaw("Vertical");
            if (vertical != 0f) _speed = acceleration;

            var horizontal = Input.GetAxisRaw("Horizontal");
            if (horizontal != 0f) _rotate = steering * horizontal;

            _currentSpeed = Mathf.SmoothStep(_currentSpeed, _speed, Time.deltaTime * 12f);
            _speed = 0f; // TODO: check if necessary.

            _currentRotate = Mathf.Lerp(_currentRotate, _rotate, Time.deltaTime * 4f);
            _rotate = 0f; // TODO: check if necessary.
        }

        private void FixedUpdate()
        {
            // Acceleration.
            sphere.AddForce(display.forward * _currentSpeed, ForceMode.Acceleration);

            // Gravity.
            sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

            // Steering.
            display.eulerAngles = Vector3.Lerp(display.eulerAngles,
                new Vector3(0f, display.eulerAngles.y + _currentRotate, 0f),
                Time.deltaTime * 5f);
        }
    }
}