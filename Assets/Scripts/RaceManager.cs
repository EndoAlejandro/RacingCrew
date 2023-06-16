using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VehicleComponents;
using Random = UnityEngine.Random;

public class RaceManager : MonoBehaviour
{
    public event Action OnGo;

    [SerializeField] private VehiclePhysics vehiclePrefab;
    [SerializeField] private VehicleController vehicleControllerPrefab;
    [SerializeField] private GameObject[] models;
    [SerializeField] private PlayerControllerInput[] playerControllers;
    [SerializeField] private Transform[] spawnPoints;

    private readonly List<Racer> _racers = new List<Racer>();
    private readonly List<VehicleController> _controllers = new List<VehicleController>();

    private const int CarsAmount = 10;

    private void Start()
    {
        //TODO: Call or receive data from GameManager.

        CreateRacersList();
        CreateVehicles();

        StartCoroutine(GoCountDown());
    }

    private void CreateVehicles()
    {
        for (int i = 0; i < _racers.Count; i++)
        {
            IControllerInput controllerInput =
                i < playerControllers.Length ? playerControllers[i] : new AiControllerInput();

            var spawnTransform = spawnPoints[i].transform;
            var model = models[Random.Range(0, models.Length)];

            var vehicleController =
                Instantiate(vehicleControllerPrefab, spawnTransform.position, spawnTransform.rotation);
            vehicleController.Setup(this, _racers[i], model, controllerInput);

            _controllers.Add(vehicleController);
        }
    }

    private IEnumerator GoCountDown()
    {
        yield return new WaitForSeconds(5f);
        OnGo?.Invoke();
    }

    private void CreateRacersList()
    {
        for (int i = 0; i < CarsAmount; i++)
        {
            var index = Random.Range(0, models.Length);
            _racers.Add(new Racer());
        }
    }
}