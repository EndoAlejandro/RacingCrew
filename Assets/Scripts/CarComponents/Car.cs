using System.Collections;
using CupComponents;
using CustomUtils;
using InputManagement;
using RaceComponents;
using Unity.Mathematics;
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
        public float NormalizedSpeed => Rigidbody.velocity.magnitude / Stats.MaxSpeed;

        private bool _canGo;
        public bool OnReset { get; private set; }

        private ICarControllerInput _carController;
        public Rigidbody Rigidbody { get; private set; }

        private void Awake()
        {
            Input = new Vector3();
            Rigidbody = GetComponent<Rigidbody>();
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
            {
                _carController = Racer.PlayerInputSingle.VehicleInputReader;
                Racer.PlayerInputSingle.OnInputTriggered += PlayerInputSingleOnInputTriggered;
            }
            else
            {
                _carController = gameObject.AddComponent<AiCarControllerInput>();
            }

            Instantiate(racer.CarModel, carContainer);
        }

        private void PlayerInputSingleOnInputTriggered()
        {
            if (((VehicleInputReader)_carController).ResetPosition && !OnReset)
                StartCoroutine(ResetPositionAsync());
        }

        public IEnumerator ResetPositionAsync()
        {
            gameObject.layer = LayerMask.NameToLayer("AvoidCars");
            OnReset = true;
            Rigidbody.isKinematic = true;
            yield return null;
            var checkPoint = TrackManager.Instance.GetNextCheckPoint(RacerPosition.LastPointIndex - 1);

            var normalizedPosition =
                NavigationRoute.Instance.GetSplineNormalizedPosition(checkPoint.transform.position);
            NavigationRoute.Instance.EvaluateSpline(normalizedPosition,
                out float3 checkPointPosition, out float3 checkPointRotation, out float3 checkPointUp);
            var position = checkPointPosition;

            transform.position = position;
            transform.rotation = Quaternion.LookRotation(checkPointRotation, checkPointUp);
            yield return null;
            Rigidbody.isKinematic = false;
            yield return new WaitForSeconds(2f);
            OnReset = false;
            gameObject.layer = LayerMask.NameToLayer("Vehicle");
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