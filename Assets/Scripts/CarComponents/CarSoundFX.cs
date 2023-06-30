using UnityEngine;

namespace CarComponents
{
    [RequireComponent(typeof(AudioSource))]
    public class CarSoundFX : CarComponent
    {
        private AudioSource _audioSource;
        private float _tForVolume;

        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (!car.Racer.IsPlayer) _audioSource.enabled = false;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        private void Update() => _audioSource.volume = Mathf.Lerp(0.1f, 1, car.NormalizedSpeed);
    }
}