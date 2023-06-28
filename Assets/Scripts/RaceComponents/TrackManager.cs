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

        [SerializeField] private int laps = 3;
        [SerializeField] private Car carPrefab;
        [SerializeField] private Transform[] spawnPoints;

        private List<CheckPoint> _checkPoints;
        public List<RacerPosition> RacersPositions { get; private set; } = new();

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
            RacersPositions.Sort();
            RacersPositions.Reverse();
        }

        private void CreateVehicles()
        {
            var racers = CupManager.Instance.CupRacers;

            for (int i = 0; i < racers.Count; i++)
            {
                var spawnTransform = spawnPoints[i].transform;
                var car = Instantiate(carPrefab, spawnTransform.position, spawnTransform.rotation);
                car.transform.SetParent(transform, true);
                car.name = "Car_" + i;

                var racer = racers[i];
                var position = new RacerPosition(racer, car);
                car.Setup(racer, position);
                RacersPositions.Add(position);
            }
        }

        public CheckPoint GetNextCheckPoint(int index) => _checkPoints[(index + 1) % _checkPoints.Count];

        public void CarThroughCheckPoint(CheckPoint checkPoint, Car car)
        {
            var index = _checkPoints.IndexOf(checkPoint);
            car.RacerPosition.SetLastPointIndex(index);

            var currentPlayersFinished = 0;
            foreach (var racerPosition in RacersPositions)
            {
                var nextCheckPoint = (racerPosition.LastPointIndex + 1) % _checkPoints.Count;
                var distance = Vector3.Distance(racerPosition.Car.transform.position,
                    _checkPoints[nextCheckPoint].transform.position);
                racerPosition.SetDistanceToNextPoint(distance);

                if (racerPosition.CupRacer.IsPlayer && racerPosition.Laps > laps)
                    currentPlayersFinished++;
            }

            UpdatePositionsList();

            if (currentPlayersFinished >= PlayersManager.Instance.PlayerInputs.Count)
                EndRace();
        }

        private void EndRace()
        {
            OnRaceOver?.Invoke();
            
            for (int i = 0; i < RacersPositions.Count; i++)
            {
                var normalizedPosition = (float)i / RacersPositions.Count;
                var points = Mathf.Lerp(Constants.RACE_POINTS, 1, normalizedPosition);
                RacersPositions[i].CupRacer.AddScore(Mathf.RoundToInt(points));
            }

            CupManager.Instance.OnTrackEnded();
        }

        private IEnumerator GoCountDown()
        {
            var currentCountDown = 5;
            while (currentCountDown > 0)
            {
                yield return new WaitForSeconds(1f);
                currentCountDown--;
            }

            OnGo?.Invoke();
        }
    }
}