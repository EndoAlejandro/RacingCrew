using System;
using CarComponents;
using UnityEngine;

namespace RaceComponents
{
    public class Racer
    {
        public Guid Id { get; private set; }

        public int Score { get; private set; }

        public IControllerInput ControllerInput { get; private set; }
        public CarData CarData { get; private set; }
        public GameObject Model { get; private set; }

        public Racer(CarData carData, GameObject model, IControllerInput controllerInput)
        {
            Id = Guid.NewGuid();

            CarData = carData;
            ControllerInput = controllerInput;
            Model = model;

            Score = 0;
        }

        public void SetScore(int value) => Score = value;
        public void SetControllerInput(IControllerInput value) => ControllerInput = value;
    }
}