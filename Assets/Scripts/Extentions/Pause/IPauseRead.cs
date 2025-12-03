namespace Extentions.Pause
{
    public interface IPauseRead
    {
        bool IsPaused { get; }
        bool IsUnpaused { get; }
    }
}