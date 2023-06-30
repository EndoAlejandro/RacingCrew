namespace InputManagement
{
    public interface ICarControllerInput
    {
        float Acceleration { get; }
        float Break { get; }
        float Turn { get; }
    }
}