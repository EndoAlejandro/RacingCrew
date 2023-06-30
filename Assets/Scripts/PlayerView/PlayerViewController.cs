using CarComponents;
using Cinemachine;
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

            VirtualCamera.m_Follow = car.transform;
            VirtualCamera.m_LookAt = car.transform;

            SetLayerMask(playerInputSingle);
        }

        private void SetLayerMask(PlayerInputSingle playerInputSingle)
        {
            var cullingMask = LayerMask.NameToLayer("Player" + playerInputSingle.Input.playerIndex);
            var layer = LayerMask.NameToLayer("Player" + playerInputSingle.Input.playerIndex);

            VirtualCamera.gameObject.layer = layer;
            _camera.gameObject.layer = layer;
            playerBanner.SetLayer(layer);

            _camera.cullingMask |= 1 << cullingMask;
        }

        public void PlayTextAnimation(string text, float duration) => playerBanner.PlayAnimation(text, duration);
    }
}