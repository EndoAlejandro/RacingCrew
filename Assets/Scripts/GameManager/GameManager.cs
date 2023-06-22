using Menu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


namespace InGame
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        [SerializeField] private AllCupsAssets allCupsAssets;

        private PlayerInputManager _playerInputManager;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _playerInputManager = GetComponent<PlayerInputManager>();

            var amount = PlayerPrefs.GetInt("NumberOfPlayers");
            for (int i = 0; i < amount; i++)
            {
                _playerInputManager.JoinPlayer();
            }

            _playerInputManager.DisableJoining();
        }

        public void LoadNextScene()
        {
            int index = 0;

            for (int i = 0; i < allCupsAssets.cups.Length; i++)
            {
                if (allCupsAssets.cups[i].cupID == PlayerPrefs.GetInt("CurrentCupID"))
                {
                    index = i;
                    break;
                }
            }

            PlayerPrefs.SetInt("CurrentSpeedway", PlayerPrefs.GetInt("CurrentSpeedway") + 1);
            SceneManager.LoadScene(allCupsAssets.cups[index].speedwayNames[PlayerPrefs.GetInt("CurrentSpeedway")]);
        }

        public void SetGlobalScore(int ID, int score)
        {
            if (!PlayerPrefs.HasKey("Score" + ID))
            {
                PlayerPrefs.SetInt("Score" + ID, 0);
            }

            PlayerPrefs.SetInt("Score" + ID, PlayerPrefs.GetInt("Score" + ID) + score);
        }

        public int GetNumberOfPlayers()
        {
            return PlayerPrefs.GetInt("NumberOfPlayers");
        }
    }
}