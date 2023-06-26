namespace RaceComponents
{
    public class RacerPosition
    {
        public Racer Racer { get; private set; }
        public float DistanceToNextPoint { get; private set; }
        public int LastPointIndex { get; private set; }
        public RacerPosition(Racer racer) => Racer = racer;
        public void SetLastPointIndex(int value) => LastPointIndex = value;
        public void SetDistanceToNextPoint(float value) => DistanceToNextPoint = value;
    }
}