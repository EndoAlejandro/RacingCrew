using CustomUtils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace RaceComponents
{
    public class NavigationRoute : Singleton<NavigationRoute>
    {
        private SplineContainer _splineContainer;
        private Spline _spline;

        protected override void Awake()
        {
            base.Awake();
            _splineContainer = GetComponent<SplineContainer>();
            _spline = _splineContainer.Splines[0];
        }

        public Vector3 GetNearestPosition(Vector3 source)
        {
            SplineUtility.GetNearestPoint(_spline, source, out float3 nearest, out float t);
            return nearest;
        }

        public float GetSplineNormalizedPosition(Vector3 source)
        {
            SplineUtility.GetNearestPoint(_spline, source, out float3 nearest, out float t);
            return t;
        }

        public void EvaluateSpline(float t, out float3 position, out float3 direction, out float3 up) =>
            _spline.Evaluate(t, out position, out direction, out up);
    }
}