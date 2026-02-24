namespace GymPassLab.Interfaces
{
    public interface IGymPassStrategy
    {
        decimal CalculateCost(int hours, bool sauna, bool pool);
    }
}