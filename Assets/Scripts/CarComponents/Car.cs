using CupComponents;
using InputManagement;
using RaceComponents;
using UnityEngine;

namespace CarComponents
{
    public class Car : MonoBehaviour
    {
        [SerializeField] private CarData defaultData;
        [SerializeField] private Transform carContainer;

        public CupRacer Racer { get; private set; }
        public Vector3 Input { get; private set; }
        public CarStats Stats => Racer?.Stats ?? defaultData.Stats;
        public RacerPosition RacerPosition { get; private set; }
        public float NormalizedSpeed => _rigidbody.velocity.magnitude / Stats.MaxSpeed;

        private bool _canGo;
        private ICarControllerInput _carController;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            Input = new Vector3();
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Setup(CupRacer racer, RacerPosition racerPosition)
        {
            RacerPosition = racerPosition;

            if (TrackManager.Instance != null)
                TrackManager.Instance.OnGo += TrackManagerOnGo;
            else
                _canGo = true;

            Racer = racer;

            if (Racer.PlayerInputSingle != null)
                _carController = Racer.PlayerInputSingle.VehicleInputReader;
            else
                _carController = gameObject.AddComponent<AiCarControllerInput>();

            Instantiate(racer.CarModel, carContainer);
        }

        private void TrackManagerOnGo() => _canGo = true;

        private void Update()
        {
            if (!_canGo) return;
            Input = new Vector3(_carController.Acceleration, _carController.Turn, _carController.Break);
        }

        private void OnDestroy()
        {
            if (TrackManager.Instance == null) return;
            TrackManager.Instance.OnGo -= TrackManagerOnGo;
        }
    }
}