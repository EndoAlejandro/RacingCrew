using System;
using Cinemachine;
using UnityEngine;

namespace CarVisuals
{
    [RequireComponent(typeof(PlayerViewController))]
    public class CarCameraEffects : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private float speedToStartCameraEffects;

        [Header("Noise Settings")]
        [SerializeField] private float maxAmplitudeGain;

        [SerializeField] private float transitionSpeedNoise;

        [Header("Camera Settings")]
        [SerializeField] private Vector3 minFollowOffset;

        [SerializeField] private Vector3 maxFollowOffset;
        [SerializeField] private float transitionSpeedCamera;

        [Header("Field Of View")]
        [SerializeField] private float minFieldOfView;

        [SerializeField] private float maxFieldOfView;
        [SerializeField] private float transitionSpeedFOV;

        private PlayerViewController _playerViewController;
        private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;
        private CinemachineTransposer _transposer;

        private Rigidbody _carRigidbody;

        private float _tForNoise;
        private float _tForOffset;
        private float _tForFOV;
        private float _fovProgress;
        private bool _back;

        private void Awake() => _playerViewController = GetComponent<PlayerViewController>();

        private void Start()
        {
            _multiChannelPerlin = _playerViewController.VirtualCamera
                .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _transposer = _playerViewController.VirtualCamera.GetCinemachineComponent<CinemachineTransposer>();

            _carRigidbody = _playerViewController.Car.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _multiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(0, maxAmplitudeGain, _playerViewController.Car.NormalizedSpeed);

            _playerViewController.VirtualCamera.m_Lens.FieldOfView =
                Mathf.Lerp(minFieldOfView, maxFieldOfView, _playerViewController.Car.NormalizedSpeed);
        }

        #region Legacy

        private void AddNoise()
        {
            if (_tForNoise <= 1) _tForNoise += Time.deltaTime * transitionSpeedNoise;
            _multiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(0, maxAmplitudeGain, _tForNoise);
        }

        private void RemoveNoise()
        {
            if (_multiChannelPerlin.m_AmplitudeGain == 0) return;

            if (_tForNoise >= 0)
            {
                _tForNoise -= Time.deltaTime * transitionSpeedNoise;
            }

            _multiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(0, maxAmplitudeGain, _tForNoise);
            if (_multiChannelPerlin.m_AmplitudeGain == 0)
            {
                _tForNoise = 0;
            }
        }

        private void AddCameraOffset()
        {
            if (_tForOffset <= 1)
            {
                _tForOffset += Time.deltaTime * transitionSpeedCamera;
            }

            _transposer.m_FollowOffset =
                Vector3.Lerp(maxFollowOffset, minFollowOffset, _tForOffset);
        }

        private void RemoveCameraOffset()
        {
            if (_transposer.m_FollowOffset == maxFollowOffset) return;

            if (_tForOffset >= 0)
            {
                _tForOffset -= Time.deltaTime * transitionSpeedCamera;
            }

            _transposer.m_FollowOffset = Vector3.Lerp(maxFollowOffset, minFollowOffset, _tForOffset);

            if (_transposer.m_FollowOffset == maxFollowOffset)
                _tForOffset = 0;
        }

        private void AddCameraFOV()
        {
            if (_tForFOV <= 1)
            {
                _tForFOV += Time.deltaTime * transitionSpeedFOV;
            }

            _playerViewController.VirtualCamera.m_Lens.FieldOfView =
                Mathf.Lerp(maxFieldOfView, minFieldOfView, _tForFOV);
        }

        private void RemoveCameraFOV()
        {
            if (Math.Abs(_playerViewController.VirtualCamera.m_Lens.FieldOfView - maxFieldOfView) < 0.1f) return;

            if (_tForFOV >= 0) _tForFOV -= Time.deltaTime * transitionSpeedFOV;

            _playerViewController.VirtualCamera.m_Lens.FieldOfView =
                Mathf.Lerp(maxFieldOfView, minFieldOfView, _tForFOV);

            if (Math.Abs(_playerViewController.VirtualCamera.m_Lens.FieldOfView - maxFieldOfView) < 0.1f)
            {
                _tForFOV = 0;
            }
        }

        #endregion
    }
}