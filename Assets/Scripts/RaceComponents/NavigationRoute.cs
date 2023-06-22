using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace RaceComponents
{
    public class NavigationRoute : MonoBehaviour
    {
        private SplineContainer _splineContainer;

        [Range(0f, 1f)] [SerializeField] private float interpolation = 5f;
        [SerializeField] private Transform marker;
        [SerializeField] private Transform visuals;

        private void Awake() => _splineContainer = GetComponent<SplineContainer>();

        private void Update()
        {
            var spline = _splineContainer.Splines[0];
            /*spline.Evaluate(interpolation, out float3 position, out float3 direction, out float3 up);
            var rotation = Quaternion.LookRotation(direction, up);
            visuals.rotation = rotation;
            visuals.position = position;*/

            var result = SplineUtility.GetNearestPoint(spline, marker.position, out float3 nearest, out float t);
            interpolation = t;
            visuals.position = nearest;
            Debug.Log(result);
        }
    }
}