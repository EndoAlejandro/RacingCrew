using UnityEngine;

namespace CarComponents
{
    public class CarParticles : CarComponent
    {
        [SerializeField] private ParticleSystem airParticleSystemFront;
        private float _dotProductResult;

        private void Update()
        {
            if (car.NormalizedSpeed > 0.7f)
            {
                if (!airParticleSystemFront.isPlaying) airParticleSystemFront.Play();
            }
            else airParticleSystemFront.Stop();
        }
    }
}