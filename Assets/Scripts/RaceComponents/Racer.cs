using System;
using CarComponents;
using UnityEngine;

namespace RaceComponents
{
    public class Racer : IEquatable<Racer>, IComparable<Racer>
    {
        public Guid Id { get; private set; }

        public int Score { get; private set; }

        public IControllerInput ControllerInput { get; private set; }
        public CarData CarData { get; private set; }
        public GameObject Model { get; private set; }
        public RacerPosition RacerPosition { get; private set; }
        public Car Car { get; private set; }

        public Racer(CarData carData, GameObject model, IControllerInput controllerInput)
        {
            Id = Guid.NewGuid();

            CarData = carData;
            ControllerInput = controllerInput;
            Model = model;

            Score = 0;

            RacerPosition = new RacerPosition(this);
        }

        public void SetScore(int value) => Score = value;
        public void SetControllerInput(IControllerInput value) => ControllerInput = value;
        public void SetCar(Car value) => Car = value;

        public bool Equals(Racer other) => other != null && Id.Equals(other.Id);

        public int CompareTo(Racer other)
        {
            if (other == null) return 1;
            var filterA = RacerPosition.LastPointIndex.CompareTo(other.RacerPosition.LastPointIndex);

            return filterA != 0f
                ? filterA
                : other.RacerPosition.DistanceToNextPoint.CompareTo(RacerPosition.DistanceToNextPoint);
        }
    }
}