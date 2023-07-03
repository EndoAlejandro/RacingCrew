using System;
using System.Collections.Generic;
using CustomUtils;
using InputManagement;
using Menu.ScriptableObjects;

namespace CupComponents
{
    public class CupManager : Singleton<CupManager>
    {
        public event Action<bool> OnLoading;
        public event Action TrackEnded;

        public List<CupRacer> CupRacers { get; private set; }

        private CupSelectionAssets _currentCup;
        private bool _playerResponse;
        public int CurrentRaceIndex { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            CupRacers = new List<CupRacer>();
        }

        private void Start()
        {
            _currentCup = GameManager.Instance.CurrentCup;
            CurrentRaceIndex = 0;

            CreateCupRacers();
            LoadTrack();
        }

        private void LoadTrack()
        {
            OnLoadingBegin();
            GameManager.Instance.LoadTrack(_currentCup.TracksData[CurrentRaceIndex].sceneName, OnLoadingEnded);
        }

        private void OnLoadingBegin() => OnLoading?.Invoke(true);
        private void OnLoadingEnded() => OnLoading?.Invoke(false);

        private void CreateCupRacers()
        {
            var racersAmount = GameManager.Instance.FixedRacersAmount;
            for (int i = 0; i < racersAmount; i++)
            {
                var inputSingle = PlayersManager.Instance.TryGetPlayerInputSingle(i);
                var racer = new CupRacer(i, inputSingle);
                CupRacers.Add(racer);
            }

            CupRacers.Reverse();
        }

        public void OnTrackEnded()
        {
            CupRacers.Sort();
            CupRacers.Reverse();
            TrackEnded?.Invoke();
            GameManager.Instance.LoadResultsScene();
        }

        public void LoadNextTrack()
        {
            CurrentRaceIndex++;
            if (CurrentRaceIndex < _currentCup.TracksData.Length)
                LoadTrack();
            else
                GameManager.Instance.LoadMainMenu();
        }
    }
}