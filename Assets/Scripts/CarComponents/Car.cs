using RaceComponents;
using UnityEngine;

namespace CarComponents
{
    public class Car : MonoBehaviour
    {
        [SerializeField] private CarData defaultData;
        public Racer Racer { get; private set; }
        private IControllerInput _controller;
        public Vector3 Input { get; private set; }
        public bool CanGo { get; private set; }

        public CarStats Stats => Racer?.CarData != null ? Racer.CarData.Stats : defaultData.Stats;

        private void Awake() => Input = new Vector3();

        public void Setup(Racer racer)
        {
            if (TrackManager.Instance != null)
                TrackManager.Instance.OnGo += TrackManagerOnGo;
            else
                CanGo = true;

            Racer = racer;
            _controller = Racer.ControllerInput;
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