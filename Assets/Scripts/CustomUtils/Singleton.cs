using UnityEngine;

namespace CustomUtils
{
    /// <summary>
    /// Base Singleton to inherit from any class. If the inheritor overrides Awake() remember to call base.Awake() at the beginning of the method. 
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T;
        }
    }
}