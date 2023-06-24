using Menu;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomUtils;

namespace InGame {
	public class GameManager : Singleton<GameManager>
	{
		public delegate void RaceStates();
		public event RaceStates RacePaused;
		public event RaceStates RaceResume;
		public event RaceStates RaceFinished;

		[SerializeField] private AllCupsAssets allCupsAssets;

		//ID del jugador || Index del carro seleccionado
		private Dictionary<int, int> _playerCarSelection = new();
		//ID del jugador || Puntuación
		private Dictionary<int, int> _globalScore = new();
		//ID del jugador || Posición final del jugador
		private Dictionary<int,int> _finalPositionInRace = new();

		public int CurrentCup { get; set; }
		public int NumberOfPlayers { get; set; }
		public int CurrentSpeedway{ get; set; }


		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(gameObject);
		}

		//Guarda la información del carro seleccionado
		public void SetPlayerCarData(int playerID, int carIndex) {
			_playerCarSelection.Add(playerID, carIndex);
		}

		public int GetPlayerCarData(int playerID) { 
			return _playerCarSelection[playerID];
		}


		//Guarda la puntuación de todos los jugadores
		public void SetGlobalScore(int id, int score) {

			int currentScore;
			_globalScore.TryGetValue(id, out currentScore);
			_globalScore.Add(id,currentScore+score);

		}

		public int GetGlobalScore(int id) {
			return _globalScore[id];
		}

		//Guarda la posición en la que todos los jugadores quedaron
		public void SetPositionInRace(int id, int position) { 
			_finalPositionInRace.Add(id,position);
		}

		public int GetPositionInRace(int id) { 
			return _finalPositionInRace[id];
		}


		
		public void PauseRace() {
			RacePaused?.Invoke();
			Time.timeScale = 0;
		}

		public void ContinueRace() { 
			RaceResume?.Invoke();
			Time.timeScale = 1;
		}

		public void LoadNextSpeedway() {
			int index = 0;
			int indexSceneToLoad = CurrentSpeedway;
			CurrentSpeedway++;
			for (int i = 0; i < allCupsAssets.cups.Length; i++)
			{
				if (allCupsAssets.cups[i].cupID == CurrentCup)
				{
					index = i;
					break;
				}
			}
			SceneManager.LoadScene(allCupsAssets.cups[index].speedwayNames[indexSceneToLoad]);
		}

	}
}
