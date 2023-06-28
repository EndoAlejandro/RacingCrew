using CupComponents;

namespace RaceComponents
{
    public class RacerPosition
    {
        public CupRacer Racer { get; private set; }
        public float DistanceToNextPoint { get; private set; }
        public int LastPointIndex { get; private set; }
        public RacerPosition(CupRacer racer) => Racer = racer;
        public void SetLastPointIndex(int value) => LastPointIndex = value;
        public void SetDistanceToNextPoint(float value) => DistanceToNextPoint = value;
    }
}