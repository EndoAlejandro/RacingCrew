using CarComponents;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace CupComponents
{
    [Serializable]
    public class CupRacer
    {
        public PlayerInputSingle PlayerInputSingle { get; private set; }
        public int RacerIndex { get; private set; }
        public int Score { get; private set; }
        public int FinalPosition { get; private set; }
        public GameObject CarModel { get; private set; }
        public CarStats Stats { get; private set; }

        public CupRacer(int racerIndex, PlayerInputSingle playerInputSingle)
        {
            RacerIndex = racerIndex;
            PlayerInputSingle = playerInputSingle;

            var defaultData = GameManager.Instance.DefaultCarData;

            if (playerInputSingle != null && playerInputSingle.CarModel != null)
            {
                CarModel = playerInputSingle.CarModel;
            }
            else
            {
                var i = Random.Range(0, defaultData.Models.Length);
                CarModel = defaultData.Models[i];
            }

            Stats = playerInputSingle != null ? playerInputSingle.CarStats : defaultData.Stats;
        }

        public void SetFinalPosition(int value) => FinalPosition = value;
        public void SetScore(int value) => Score = value;
    }
}