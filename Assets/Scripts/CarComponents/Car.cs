using CupComponents;
using RaceComponents;
using UnityEngine;

namespace CarComponents
{
    public class Car : MonoBehaviour
    {
        [SerializeField] private CarData defaultData;

        private bool _canGo;
        private IControllerInput _controller;

        public CupRacer Racer { get; private set; }
        public Vector3 Input { get; private set; }
        public CarStats Stats => Racer?.Stats ?? defaultData.Stats;

        public RacerPosition RacerPosition { get; private set; }

        private void Awake() => Input = new Vector3();

        public void Setup(CupRacer racer, RacerPosition racerPosition)
        {
            RacerPosition = racerPosition;

            if (TrackManager.Instance != null)
                TrackManager.Instance.OnGo += TrackManagerOnGo;
            else
                _canGo = true;

            Racer = racer;

            if (Racer.PlayerInputSingle != null)
                _controller = Racer.PlayerInputSingle.VehicleInputReader;
            else
                _controller = gameObject.AddComponent<AiControllerInput>();

            Instantiate(racer.CarModel, transform);
        }

        private void TrackManagerOnGo() => _canGo = true;

        private void Update()
        {
            if (!_canGo) return;
            Input = new Vector3(_controller.Acceleration, _controller.Turn, _controller.Break);
        }

        private void OnDestroy()
        {
            if (TrackManager.Instance == null) return;
            TrackManager.Instance.OnGo -= TrackManagerOnGo;
        }
    }
}