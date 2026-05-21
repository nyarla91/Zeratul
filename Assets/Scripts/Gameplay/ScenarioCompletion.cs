using System;
using UnityEngine;

namespace Gameplay
{
    public class ScenarioCompletion : IScenarioCompleteService, IScenarioCompletionInfo
    {
        public event Action Completed;
        
        public void Complete() => Completed?.Invoke();
    }

    public interface IScenarioCompleteService
    {
        void Complete();
    }

    public interface IScenarioCompletionInfo
    {
        event Action Completed;
    }
}