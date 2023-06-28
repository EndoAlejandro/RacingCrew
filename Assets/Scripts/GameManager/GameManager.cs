using System;
using System.Collections;
using Menu;
using System.Collections.Generic;
using CarComponents;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomUtils;
using UnityEngine.InputSystem;

namespace InGame
{
    public class GameManager : Singleton<GameManager>
    {
        public delegate void RaceStates();

        public event RaceStates RacePaused;
        public event RaceStates RaceResume;
        public event RaceStates RaceFinished;

        [SerializeField] private int fixedRacersAmount = 5;
        [SerializeField] private CarData defaultCarData;
        [SerializeField] private AllCupsAssets allCupsAssets;

        private Dictionary<int, GrandPrixRacer> _cupRacers = new();

        //ID del jugador || Index del carro seleccionado
        public List<ModelAndInputPlayer> PlayerCarInformation { get; private set; } = new();

        //ID del jugador || Puntuaci�n
        private Dictionary<int, int> _globalScore = new();

        //ID del jugador || Posici�n final del jugador
        private Dictionary<int, int> _finalPositionInRace = new();
        public List<PlayerInput> Inputs { get; private set; } = new List<PlayerInput>();

        public CupSelectionAssets CurrentCup { get; private set; }

        // public int PlayersCount { get; private set; }
        public int CurrentSpeedway { get; private set; }
        public int FixedRacersAmount => fixedRacersAmount;
        public CarData DefaultCarData => defaultCarData;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public void AddPlayerCarModel(PlayerInput input, GameObject model) =>
            PlayerCarInformation.Add(new ModelAndInputPlayer(input, model));

        //Guarda la informaci�n del carro seleccionado
        /*public GameObject GetPlayerCarModel(int playerID) =>
            defaultCarData.Models[_playerCarSelection[playerID]];*/

        //Guarda la puntuaci�n de todos los jugadores
        public void SetGlobalScore(int id, int score)
        {
            int currentScore;
            _globalScore.TryGetValue(id, out currentScore);
            _globalScore.Add(id, currentScore + score);
        }

        public int GetGlobalScore(int id)
        {
            return _globalScore[id];
        }

        //Guarda la posici�n en la que todos los jugadores quedaron
        public void SetPositionInRace(int id, int position)
        {
            _finalPositionInRace.Add(id, position);
        }

        public int GetPositionInRace(int id)
        {
            return _finalPositionInRace[id];
        }

        public void PauseRace()
        {
            RacePaused?.Invoke();
            Time.timeScale = 0;
        }

        public void ContinueRace()
        {
            RaceResume?.Invoke();
            Time.timeScale = 1;
        }

        public void StartCup() => StartCoroutine(StartCupAsync());

        private IEnumerator StartCupAsync()
        {
            yield return SceneManager.LoadSceneAsync("Scenes/Cup");
        }

        public void LoadTrack(string sceneName, Action turnOff) =>
            StartCoroutine(LoadTrackAsync(sceneName, turnOff));

        private IEnumerator LoadTrackAsync(string sceneName, Action turnOff)
        {
            if (SceneManager.loadedSceneCount > 1)
            {
                var last = SceneManager.GetSceneAt(SceneManager.loadedSceneCount);
                yield return SceneManager.UnloadSceneAsync(last);
            }

            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            turnOff?.Invoke();
        }

        public void LoadNextSpeedway()
        {
            CurrentSpeedway++;
            SceneManager.LoadScene(CurrentCup.TracksData[CurrentSpeedway - 1].sceneName);
        }

        public void SetCurrentCup(CupSelectionAssets cupSelectionAssets) => CurrentCup = cupSelectionAssets;

        // public void SetPlayersCount(int value) => PlayersCount = value;
        public void AddPlayerInput(PlayerInput input) => Inputs.Add(input);
    }
}