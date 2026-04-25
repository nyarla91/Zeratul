using Cysharp.Threading.Tasks;
using GameState.States;
using UnityEditor.Overlays;
using UnityEngine;
using Zenject;
using SaveData = Saving.Data.SaveData;

namespace GameState
{
    public class GameFlowController
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ScenarioSession _scenarioSession;

        [Inject]
        public GameFlowController(GameStateMachine stateMachine, ScenarioSession scenarioSession)
        {
            _stateMachine = stateMachine;
            _scenarioSession = scenarioSession;
            Debug.Log(this);
            LaunchGame();
        }

        public void StartScenarioFromSaveData(SaveData saveData)
        {
            _scenarioSession.SetSaveData(saveData);
            _stateMachine.Enter<GameplayState>();
        }

        public void StartScenario(ScenarioData scenario)
        {
            if (_stateMachine.CurrentState is GameplayState)
                return;
            _scenarioSession.Set(scenario);
            _stateMachine.Enter<GameplayState>();
        }

        public void RestartScenario()
        {
            if (_stateMachine.CurrentState is not GameplayState)
                return;
            _stateMachine.GetState<GameplayState>().RestartScenario();
        }

        public void LeaveScenario()
        {
            if (_stateMachine.CurrentState is not GameplayState)
                return;
            _stateMachine.Enter<MainMenuState>();
        }

        private async void LaunchGame()
        {
            BootstrapState bootstrapState = _stateMachine.Enter<BootstrapState>();
            await UniTask.WaitUntil(bootstrapState, s => s.Finished);
            _stateMachine.Enter<MainMenuState>();
        }
    }
}