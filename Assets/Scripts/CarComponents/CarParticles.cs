using UnityEngine;

namespace CarComponents
{
    public class CarParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem airParticleSystemFront;

        private Car _car;
        private Rigidbody _rigidbody;
        private float _dotProductResult;

        private void Awake()
        {
            _car = GetComponent<Car>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_car.NormalizedSpeed > 0.2f)
            {
                if (!airParticleSystemFront.isPlaying) airParticleSystemFront.Play();
            }
            else airParticleSystemFront.Stop();
        }
    }
}