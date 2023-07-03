using System.Collections.Generic;
using UnityEngine;

namespace CustomUtils
{
    public static class ListExtensions
    {
        /// <summary>
        /// Randomize all the positions of a List.
        /// </summary>
        public static void Shuffle<T>(this IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = Random.Range(i, count);
                (ts[i], ts[r]) = (ts[r], ts[i]);
            }
        }
    }

    public static class Vector3Extensions
    {
        /// <summary>
        /// Override an specific axis of the Vector.
        /// </summary>
        /// <param name="One Axis"> myVector.With(y: valueY);</param>
        /// <param name="Two Axis"> myVector.With(x: valueX, y: valueY);</param>
        /// <param name="All Axis"> myVector.With(x: valueX, y: valueY, z: valueZ);</param>
        /// <returns></returns>
        public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z);
        }

        /// <summary>
        /// Add value to an specific axis of the Vector.
        /// </summary>
        /// <param name="One Axis"> myVector.Plus(y: valueY);</param>
        /// <param name="Two Axis"> myVector.Plus(x: valueX, y: valueY);</param>
        /// <param name="All Axis"> myVector.Plus(x: valueX, y: valueY, z: valueZ);</param>
        /// <returns></returns>
        public static Vector3 Plus(this Vector3 original, float? x = null, float? y = null, float? z = null)
        {
            float newX = (float)(x != null ? original.x + x : original.x);
            float newY = (float)(y != null ? original.y + y : original.y);
            float newZ = (float)(z != null ? original.z + z : original.z);
            return new Vector3(newX, newY, newZ);
        }
    }

    public static class Vector2Extensions
    {
        /// <summary>
        /// Override an specific axis of the Vector.;
        /// </summary>
        /// <param name="One Axis"> myVector.With(y: valueY);</param>
        /// <param name="All Axis"> myVector.With(x: valueX, y: valueY);</param>
        /// <returns></returns>
        public static Vector2 With(this Vector2 original, float? x = null, float? y = null) =>
            new(x ?? original.x, y ?? original.y);

        /// <summary>
        /// Get a random float from a Vector2.
        /// </summary>
        /// <param name="original">x = min Value | y = max Value.</param>
        /// <returns></returns>
        public static float GetRandomPointInRange(this Vector2 original) =>
            Random.Range(original.x, original.y);

        /// <summary>
        /// Get a float value between a Vector2 axis.
        /// </summary>
        /// <param name="original">x = min Value | y = max Value.</param>
        /// <param name="normalizedValue">Value must be between 0 and 1. 0 = x | 1 = y</param>
        /// <returns></returns>
        public static float GetPointInRange(this Vector2 original, float normalizedValue) =>
            Mathf.Lerp(original.x, original.y, normalizedValue);

        /// <summary>
        /// Get a float value between a Vector2 axis.
        /// </summary>
        /// <param name="original">x = min Value | y = max Value.</param>
        /// <param name="normalizedValue">Value must be between 0 and 1. 0 = x | 1 = y</param>
        /// <returns></returns>
        public static float GetPointInRange(this Vector2Int original, float normalizedValue) =>
            Mathf.Lerp(original.x, original.y, normalizedValue);
    }
}