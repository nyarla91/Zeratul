using System;
using Newtonsoft.Json;

namespace _Core
{
    [Serializable]
    public class Objective
    {
        [JsonProperty] public string Label { get; private set; }
        [JsonProperty] public int Counter { get; private set; }
        [JsonProperty] public int Goal { get; private set; }
        [JsonProperty] public ObjectiveStatus Status { get; set; }

        public Objective(string label, int counter, int goal, ObjectiveStatus status = ObjectiveStatus.Active)
        {
            Label = label;
            Counter = counter;
            Goal = goal;
            Status = status;
        }

        public void Increment(int value)
        {
            if (value <= 0)
                return;
            Counter = Math.Clamp(Counter + value, 0, Goal);
        }

        public void UpdateCurrentCounter(int value)
        {
            Counter = Math.Clamp(value, 0, Goal);
        }
    }

    public enum ObjectiveStatus
    {
        Active,
        Completed,
        Failed
    }
}