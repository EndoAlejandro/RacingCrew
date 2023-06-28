using System;
using CarComponents;
using CupComponents;

namespace RaceComponents
{
    public class RacerPosition : IEquatable<RacerPosition>, IComparable<RacerPosition>
    {
        public CupRacer CupRacer { get; private set; }
        public Car Car { get; private set; }
        public float DistanceToNextPoint { get; private set; }
        public int LastPointIndex { get; private set; }
        public int Laps { get; private set; }

        public RacerPosition(CupRacer cupRacer, Car car)
        {
            CupRacer = cupRacer;
            Car = car;
        }

        public void SetLastPointIndex(int value)
        {
            LastPointIndex = value;
            if (LastPointIndex == 0) AddLap();
        }

        public void SetDistanceToNextPoint(float value) => DistanceToNextPoint = value;
        private void AddLap() => Laps++;

        public bool Equals(RacerPosition other) =>
            other != null && CupRacer.RacerIndex.Equals(other.CupRacer.RacerIndex);

        public int CompareTo(RacerPosition other)
        {
            if (other == null) return 1;

            // First Check laps
            var filterA = Laps.CompareTo(other.Laps);
            if (filterA != 0) return filterA;

            // Then Check checkpoints
            var filterB = LastPointIndex.CompareTo(other.LastPointIndex);
            if (filterB != 0) return filterB;

            // Then check for distance to the next check point.
            return other.DistanceToNextPoint.CompareTo(DistanceToNextPoint);
        }
    }
}