using System;
using CarComponents;
using UnityEngine;

namespace CarVisuals
{
    [RequireComponent(typeof(AudioSource))]
    public class CarSoundFX : MonoBehaviour
    {
        [SerializeField] private float speedVolumeTransitionUp = 0.25f;
        [SerializeField] private float speedVolumeTransitionDown = 0.5f;

        private Car _car;
        private AudioSource _audioSource;
        private Rigidbody _rigidbody;
        private float _tForVolume;

        private void Awake()
        {
            _car = GetComponent<Car>();
            _audioSource = GetComponent<AudioSource>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            if (!_car.Racer.IsPlayer) _audioSource.enabled = false;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        private void Update()
        {
            _audioSource.volume = Mathf.Lerp(0, 1, _car.NormalizedSpeed);
            /*if (_rigidbody.velocity.magnitude > 5)
            {
                if (_tForVolume <= 1)
                {
                    _tForVolume += Time.deltaTime * speedVolumeTransitionUp;
                }

                _audioSource.volume = Mathf.Lerp(0, 1, _tForVolume);
            }

            if (_rigidbody.velocity.magnitude < 20)
            {
                if (_tForVolume >= 0)
                {
                    _tForVolume -= Time.deltaTime * speedVolumeTransitionDown;
                }

                _audioSource.volume = Mathf.Lerp(0, 1, _tForVolume);
            }*/
        }
    }
}