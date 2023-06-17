using System;
using System.Collections;
using System.Collections.Generic;
using CarComponents;
using CustomUtils;
using UnityEngine;
using UnityEngine.Serialization;
using VehicleComponents;
using Random = UnityEngine.Random;

namespace RaceComponents
{
    public class TrackManager : Singleton<TrackManager>
    {
        public event Action OnGo;

        [SerializeField] private Car carPrefab;
        [SerializeField] private CarData carData;
        [SerializeField] private CarController carControllerPrefab;

        [SerializeField] private GameObject[] models;
        [SerializeField] private PlayerControllerInput[] playerControllers;
        [SerializeField] private Transform[] spawnPoints;

        public List<Racer> Racers { get; private set; } = new List<Racer>();
        public List<Car> Cars { get; private set; } = new List<Car>();
        private readonly List<CarController> _controllers = new List<CarController>();

        private const int CarsAmount = 10;

        private void Start()
        {
            CreateRacersList();
            CreateVehicles();

            StartCoroutine(GoCountDown());
        }

        /// <summary>
        /// This method must be replaced with getting the racers list from GameManager.
        /// </summary>
        private void CreateRacersList()
        {
            for (int i = 0; i < CarsAmount; i++)
            {
                var model = models[Random.Range(0, models.Length)];
                IControllerInput controllerInput =
                    i < playerControllers.Length ? playerControllers[i] : new AiControllerInput();
                Racers.Add(new Racer(carData, model, controllerInput));
            }
        }

        private void CreateVehicles()
        {
            Cars.Clear();
            for (int i = 0; i < Racers.Count; i++)
            {
                var spawnTransform = spawnPoints[i].transform;
                var car = Instantiate(carPrefab, spawnTransform.position, spawnTransform.rotation);
                car.Setup(Racers[i]);
                Cars.Add(car);
            }
        }

        private IEnumerator GoCountDown()
        {
            yield return new WaitForSeconds(5f);
            OnGo?.Invoke();
        }
    }
}