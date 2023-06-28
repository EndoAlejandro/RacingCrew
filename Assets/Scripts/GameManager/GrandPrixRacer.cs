namespace InGame
{
    public class GrandPrixRacer
    {
        public int ModelIndex { get; private set; }
        public int Score { get; private set; }
        public int FinalPosition { get; private set; }

        public GrandPrixRacer()
        {
            Score = 0;
            ModelIndex = 0;
            FinalPosition = 0;
        }

        public void SetModelIndex(int value) => ModelIndex = value;
        public void SetFinalPosition(int value) => FinalPosition = value;
        public void SetScore(int value) => Score = value;
    }
}