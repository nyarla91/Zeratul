namespace _Core.Pause
{
    public interface IPauseReadonly
    {
        bool IsPaused { get; }
        bool IsUnpaused { get; }
    }
}