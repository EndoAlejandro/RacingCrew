using System;
using RaceComponents;
using UnityEngine;

namespace CarComponents
{
    public class Car : MonoBehaviour
    {
        private Racer _racer;
        private IControllerInput _controller;
        public Vector3 Input { get; private set; }
        public bool CanGo { get; private set; }
        public CarStats Stats => _racer?.CarData != null ? _racer
            .CarData.Stats : null;

        private void Awake() => Input = new Vector3();

        public void Setup(Racer racer)
        {
            TrackManager.Instance.OnGo += TrackManagerOnGo;

            _racer = racer;
            _controller = _racer.ControllerInput;
            Instantiate(racer.Model, transform);
        }

        private void TrackManagerOnGo() => CanGo = true;

        private void Update()
        {
            if (!CanGo) return;
            Input = new Vector3(_controller.Acceleration, _controller.Turn, _controller.Break);
        }

        private void OnDestroy()
        {
            if (TrackManager.Instance == null) return;
            TrackManager.Instance.OnGo -= TrackManagerOnGo;
        }
    }
}