using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CarComponents;
using CustomUtils;
using UnityEngine;
using UnityEngine.Serialization;

namespace RaceComponents
{
    public class TrackManager : Singleton<TrackManager>
    {
        public event Action OnGo;
        public event Action OnRaceOver;

        [SerializeField] private Car carPrefab;

        [FormerlySerializedAs("playerCameraPrefab")] [SerializeField]
        private PlayerViewController playerViewControllerPrefab;

        [SerializeField] private int laps = 3;
        [SerializeField] private Transform[] spawnPoints;

        private List<CheckPoint> _checkPoints;
        public List<RacerPosition> RacersPositions { get; private set; } = new();

        private bool _isRacing;

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

        private void UpdatePositionsList()
        {
            RacersPositions.Sort();
            RacersPositions.Reverse();
        }

        private void CreateVehicles()
        {
            var racers = CupManager.Instance.CupRacers;

            for (int i = 0; i < racers.Count; i++)
            {
                var car = CreateCar(i);

                var racer = racers[i];
                var position = new RacerPosition(racer, car);
                car.Setup(racer, position);
                RacersPositions.Add(position);

                if (!racer.IsPlayer) continue;

                var playerViewController = Instantiate(playerViewControllerPrefab, transform);
                playerViewController.Setup(racer.PlayerInputSingle, car);
            }

            PlayersManager.Instance.SetSplitScreen(true);
        }

        private Car CreateCar(int index)
        {
            var spawnTransform = spawnPoints[index].transform;
            var car = Instantiate(carPrefab, spawnTransform.position, spawnTransform.rotation);
            car.transform.SetParent(transform, true);
            car.name = "Car_" + index;
            return car;
        }

        public CheckPoint GetNextCheckPoint(int index) => _checkPoints[(index + 1) % _checkPoints.Count];

        public void CarThroughCheckPoint(CheckPoint checkPoint, Car car)
        {
            if (!_isRacing) return;

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
            PlayersManager.Instance.SetSplitScreen(false);
            OnRaceOver?.Invoke();

            for (int i = 0; i < RacersPositions.Count; i++)
            {
                var normalizedPosition = (float)i / RacersPositions.Count;
                var points = Mathf.Lerp(Constants.RACE_POINTS, 1, normalizedPosition);
                RacersPositions[i].CupRacer.AddScore(Mathf.RoundToInt(points));
            }

            _isRacing = false;
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

            _isRacing = true;
            OnGo?.Invoke();
        }
    }
}