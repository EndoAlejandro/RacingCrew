using UnityEngine;

namespace Pooling
{
    public sealed class PoolAfterSeconds : PooledMonoBehaviour
    {
        [SerializeField] private float delay;
        private void OnEnable() => ReturnToPool(delay);
    }
}