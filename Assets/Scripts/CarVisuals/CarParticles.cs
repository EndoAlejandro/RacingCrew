using UnityEngine;

namespace CarVisuals
{
    public class CarParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem airParticleSystemFront;
        [SerializeField] private ParticleSystem airParticleSystemBack;
        [SerializeField] private float speedToStartAirParticleSystem = 20;

        private Rigidbody _rigidbody;
        private float _dotProductResult;

        private void Awake() => _rigidbody = GetComponent<Rigidbody>();

        private void Update()
        {
            _dotProductResult = Vector3.Dot(transform.forward.normalized, _rigidbody.velocity.normalized);

            if (_dotProductResult > 0)
                ActivateAirParticleSystemFront();
            else
                ActivateAirParticleSystemBack();
        }

        private void ActivateAirParticleSystemFront()
        {
            if (_rigidbody.velocity.magnitude > speedToStartAirParticleSystem)
            {
                if (!airParticleSystemFront.isPlaying)
                {
                    airParticleSystemFront.Play();
                }
            }
            else
            {
                airParticleSystemFront.Stop();
            }
        }

        private void ActivateAirParticleSystemBack()
        {
            if (_rigidbody.velocity.magnitude > speedToStartAirParticleSystem)
            {
                if (!airParticleSystemBack.isPlaying)
                {
                    airParticleSystemBack.Play();
                }
            }
            else
            {
                airParticleSystemBack.Stop();
            }
        }
    }
}