using Cinemachine;
using UnityEngine;

namespace PlayerView
{
    [RequireComponent(typeof(PlayerViewController))]
    public class CarCameraEffects : MonoBehaviour
    {
        [Header("Noise Settings")]
        [SerializeField] private float maxAmplitudeGain;

        [Header("Field Of View")]
        [SerializeField] private float minFieldOfView;
        [SerializeField] private float maxFieldOfView;

        private PlayerViewController _playerViewController;
        private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;

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
        }

        private void Update()
        {
            _multiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(0, maxAmplitudeGain, _playerViewController.Car.NormalizedSpeed);

            _playerViewController.VirtualCamera.m_Lens.FieldOfView =
                Mathf.Lerp(minFieldOfView, maxFieldOfView, _playerViewController.Car.NormalizedSpeed);
        }
    }
}