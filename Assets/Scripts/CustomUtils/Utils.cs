using UnityEngine;

namespace CustomUtils
{
    public static class Utils
    {
        /// <summary>
        /// Return the direction of two Vector3 ignoring the Y value.
        /// </summary>
        /// <param name="to"> Target vector</param>
        /// <param name="from"> Source vector.</param>
        /// <returns>Vector3</returns>
        public static Vector3 FlatDirection(Vector3 to, Vector3 from)
        {
            to.y = 0f;
            from.y = 0f;
            return to - from;
        }

        /// <summary>
        /// Return the normalized direction of two Vector3 ignoring the Y value.
        /// </summary>
        /// <param name="to"> Target vector</param>
        /// <param name="from"> Source vector.</param>
        /// <returns>Vector3 with magnitude = 1.</returns>
        public static Vector3 NormalizedFlatDirection(Vector3 to, Vector3 from) => FlatDirection(to, from).normalized;

        /// <summary>
        /// Return the normalized Dot product of two Vector3.
        /// </summary>
        /// <param name="lhs">Vector3</param>
        /// <param name="rhs">Vector3</param>
        /// <returns>float</returns>
        public static float NormalizedDotProduct(Vector3 lhs, Vector3 rhs)
        {
            var dot = Vector3.Dot(lhs, rhs);
            return (dot + 1) / 2f;
        }
    }
}