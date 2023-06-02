using System;
using UnityEngine;

namespace VehicleComponents
{
    public class WheelPhysics : MonoBehaviour
    {
        private VehiclePhysics _vehiclePhysics;

        private RaycastHit _hit;
        private Vector3 _force;

        private void Awake()
        {
            _vehiclePhysics = GetComponentInParent<VehiclePhysics>();
            _hit = new RaycastHit();
        }

        private void FixedUpdate()
        {
            if (!Physics.Raycast(transform.position, -_vehiclePhysics.transform.up, out _hit,
                    _vehiclePhysics.SuspensionDistance)) return;
            
            var springDirection = transform.up;

            // Get velocity of a point relative to rigidbody.
            var tireWorldVelocity = _vehiclePhysics.Rigidbody.GetPointVelocity(transform.position);

            var offset = _vehiclePhysics.SuspensionDistance - _hit.distance;

            var velocity = Vector3.Dot(springDirection, tireWorldVelocity);
            var force = (offset * _vehiclePhysics.SpringStrength) - (velocity * _vehiclePhysics.Damp);

            _force = springDirection * force;
            _vehiclePhysics.Rigidbody.AddForceAtPosition(_force, transform.position);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + _force);
        }
    }
}