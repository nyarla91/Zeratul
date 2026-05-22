using Cysharp.Threading.Tasks;
using GameState.States;
using UnityEngine;
using Zenject;
using SaveData = Save.Data.SaveData;

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

        public void StartScenario(int scenarioId)
        {
            if (_stateMachine.CurrentState is GameplayState)
                return;
            _scenarioSession.ClearSaveData();
            _scenarioSession.Set(scenarioId);
            _stateMachine.Enter<GameplayState>();
        }

        public void RestartScenario()
        {
            if (_stateMachine.CurrentState is not GameplayState)
                return;
            _scenarioSession.ClearSaveData();
            _stateMachine.GetState<GameplayState>().RestartScenario();
        }

        public void LeaveScenario()
        {
            if (_stateMachine.CurrentState is not GameplayState)
                return;
            _scenarioSession.ClearSaveData();
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