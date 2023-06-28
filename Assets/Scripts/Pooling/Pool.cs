using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class Pool : MonoBehaviour
    {
        private static readonly Dictionary<PooledMonoBehaviour, Pool> Pools = new Dictionary<PooledMonoBehaviour, Pool>();

        private readonly Queue<PooledMonoBehaviour> _objects = new Queue<PooledMonoBehaviour>();

        private PooledMonoBehaviour _prefab;

        public static Pool GetPool(PooledMonoBehaviour prefab)
        {
            if (Pools.ContainsKey(prefab)) return Pools[prefab];

            var pool = new GameObject("Pool-" + prefab.name).AddComponent<Pool>();
            pool._prefab = prefab;

            Pools.Add(prefab, pool);
            return pool;
        }

        public T Get<T>() where T : PooledMonoBehaviour
        {
            if (_objects.Count == 0) GrowPool();

            var pooledObject = _objects.Dequeue();
            return pooledObject as T;
        }

        private void GrowPool()
        {
            for (int i = 0; i < _prefab.InitialPoolSize; i++)
            {
                var pooledObject = Instantiate(_prefab, this.transform, true) as PooledMonoBehaviour;
                pooledObject.name += " " + i;
                pooledObject.OnReturnToPool += AddToQueue;
                pooledObject.gameObject.SetActive(false);
            }
        }

        private void AddToQueue(PooledMonoBehaviour pooledObject)
        {
            pooledObject.transform.SetParent(this.transform);
            _objects.Enqueue(pooledObject);
        }

        private void OnDestroy()
        {
            Pools.Clear();
            while (_objects.Count > 0)
            {
                var o = _objects.Dequeue();
                o.OnReturnToPool -= AddToQueue;
                Destroy(o.gameObject);
            }
        }
    }
}