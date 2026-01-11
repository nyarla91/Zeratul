namespace Extentions.Pause
{
    public interface IPauseGet
    {
        bool IsPaused { get; }
        bool IsUnpaused { get; }
    }
}