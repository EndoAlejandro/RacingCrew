using UnityEngine;

namespace CarComponents
{
    [RequireComponent(typeof(AudioSource))]
    public class CarSoundFX : MonoBehaviour
    {
        private Car _car;
        private AudioSource _audioSource;
        private float _tForVolume;

        private void Awake()
        {
            _car = GetComponent<Car>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (!_car.Racer.IsPlayer) _audioSource.enabled = false;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        private void Update() => _audioSource.volume = Mathf.Lerp(0.1f, 1, _car.NormalizedSpeed);
    }
}