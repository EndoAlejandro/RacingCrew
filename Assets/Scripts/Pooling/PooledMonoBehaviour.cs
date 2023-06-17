using System;
using System.Collections;
using UnityEngine;

namespace Pooling
{
    public class PooledMonoBehaviour : MonoBehaviour {
    
        [Header("Pooling")]
        [SerializeField] private int initialPoolSize = 50;

        public event Action<PooledMonoBehaviour> OnReturnToPool;
        public int InitialPoolSize => initialPoolSize;

        public T Get<T>(bool enable = true) where T : PooledMonoBehaviour {
            var pool = Pool.GetPool(this);
            var pooledObject = pool.Get<T>();

            if (enable)
                pooledObject.gameObject.SetActive(true);

            return pooledObject;
        }

        public T Get<T>(Vector3 position, Quaternion rotation) where T : PooledMonoBehaviour {
            var pooledObject = Get<T>();

            pooledObject.transform.position = position;
            pooledObject.transform.rotation = rotation;

            return pooledObject;
        }

        protected virtual void OnDisable() {
            OnReturnToPool?.Invoke(this);
        }

        protected internal void ReturnToPool(float delay = 0f) {
            StartCoroutine(ReturnToPoolAfterSeconds(delay));
        }

        private IEnumerator ReturnToPoolAfterSeconds(float delay) {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
        }
    }
}
