namespace CarComponents
{
    public class AiControllerInput : IControllerInput
    {
        public float Acceleration => 1f;
        public float Break => 0f;
        public float Turn => 0f;
    }
}