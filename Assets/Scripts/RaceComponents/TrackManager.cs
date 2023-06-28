using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CarComponents;
using CustomUtils;
using UnityEngine;

namespace RaceComponents
{
    public class TrackManager : Singleton<TrackManager>
    {
        public event Action OnGo;
        public event Action OnRaceOver;

        [SerializeField] private Car carPrefab;

        [SerializeField] private Transform spawnPointsContainer;
        [SerializeField] private Transform[] spawnPoints;

        public List<Car> Cars { get; private set; } = new List<Car>();

        // private Dictionary<Racer, Car> _carRacer = new Dictionary<Racer, Car>();

        private List<CheckPoint> _checkPoints;

        protected override void Awake()
        {
            base.Awake();
            _checkPoints = transform.GetComponentsInChildren<CheckPoint>().ToList();
        }

        private void Start()
        {
            CreateVehicles();
            StartCoroutine(GoCountDown());
        }

        public void UpdatePositionsList()
        {
            /*Racers.Sort();
            Racers.Reverse();*/
        }

        /// <summary>
        /// This method must be replaced with getting the racers list from GameManager.
        /// </summary>
        /*private void CreateRacersList()
        {
            for (int i = 0; i < _carsAmount; i++)
            {
                var model = models[Random.Range(0, models.Length)];
                IControllerInput controllerInput =
                    i < playerControllers.Length ? playerControllers[i] : CreateNewAiController(i);

                var racer = new Racer(carData, model, controllerInput);
                Racers.Add(racer);
            }
        }*/
        private AiControllerInput CreateNewAiController(int index)
        {
            var controller = new GameObject("AIController_" + index).AddComponent<AiControllerInput>();
            controller.Setup(index);
            return controller;
        }

        private void CreateVehicles()
        {
            var racers = CupManager.Instance.Racers;
            
            for (int i = 0; i < racers.Count; i++)
            {
                var spawnTransform = spawnPoints[i].transform;
                var car = Instantiate(carPrefab, spawnTransform.position, spawnTransform.rotation);
                car.name = "Car_" + i;
                
                var racer = racers[i];
                car.Setup(racer);
                Cars.Add(car);
            }
        }

        public CheckPoint GetNextCheckPoint(int index) => _checkPoints[(index + 1) % _checkPoints.Count];

        public void CarThroughCheckPoint(CheckPoint checkPoint, Car car)
        {
            /*var index = _checkPoints.IndexOf(checkPoint);
            car.Racer.RacerPosition.SetLastPointIndex(index);

            foreach (var c in Cars)
            {
                var nextCheckPoint = (c.Racer.RacerPosition.LastPointIndex + 1) % _checkPoints.Count;
                var distance = Vector3.Distance(c.transform.position, _checkPoints[nextCheckPoint].transform.position);
                c.Racer.RacerPosition.SetDistanceToNextPoint(distance);
            }

            UpdatePositionsList();*/
        }

        private IEnumerator GoCountDown()
        {
            yield return new WaitForSeconds(1f);
            OnGo?.Invoke();
        }
    }
}