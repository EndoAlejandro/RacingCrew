using CarComponents;
using Cinemachine;
using InputManagement;
using RaceComponents;
using UnityEngine;

namespace PlayerView
{
    public class PlayerViewController : MonoBehaviour
    {
        [SerializeField] private PlayerViewAnimatedText playerBanner;
        private Camera _camera;
        private Canvas _canvas;
        public CinemachineVirtualCamera VirtualCamera { get; private set; }
        public Car Car { get; private set; }

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _camera = GetComponentInChildren<Camera>();
            VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }

        public void Setup(PlayerInputSingle playerInputSingle, Car car)
        {
            playerInputSingle.Input.camera = _camera;

            Car = car;

            var t = car.transform;
            VirtualCamera.m_Follow = t;
            VirtualCamera.m_LookAt = t;

            SetLayerMask(playerInputSingle);
        }

        private void SetLayerMask(PlayerInputSingle playerInputSingle)
        {
            var layer = LayerMask.NameToLayer("Player" + playerInputSingle.Input.playerIndex);

            VirtualCamera.gameObject.layer = layer;
            _camera.gameObject.layer = layer;
            playerBanner.SetLayer(layer);

            _camera.cullingMask |= 1 << layer;
        }

        public void PlayTextAnimation(string text, float duration) => playerBanner.PlayAnimation(text, duration);
    }
}