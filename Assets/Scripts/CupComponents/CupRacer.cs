using CarComponents;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace CupComponents
{
    [Serializable]
    public class CupRacer : IComparable<CupRacer>
    {
        public PlayerInputSingle PlayerInputSingle { get; private set; }
        public int RacerIndex { get; private set; }
        public int Score { get; private set; }
        public int FinalPosition { get; private set; }
        public GameObject CarModel { get; private set; }
        public CarStats Stats { get; private set; }
        public bool IsPlayer => PlayerInputSingle != null;

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
        public void AddScore(int value) => Score += value;

        public int CompareTo(CupRacer other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            var scoreComparison = Score.CompareTo(other.Score);
            return scoreComparison;
        }
    }
}