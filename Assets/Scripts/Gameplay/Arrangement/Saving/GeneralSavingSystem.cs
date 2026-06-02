using System;
using GameState;
using Save.Data;
using UniRx;
using Zenject;

namespace Gameplay.Arrangement.Saving
{
    public class GeneralSavingSystem : SavingSystem<GeneralSaveSystem>
    {
        [Inject] private ScenarioSession ScenarioSession { get; set; }
        [Inject] private GameTime GameTime { get; set; }
        [Inject] private TacticalPause TacticalPause { get; set; }
        [Inject] private TacticalPauseControl TacticalPauseControl { get; set; }

        protected override string LoadKey => GeneralSaveSystem.LoadKey;

        public override void ReproduceFromSaveData(GeneralSaveSystem payload)
        {
            GameTime.ReproduceFromSaveData(payload);
            if (payload.isTacticalPauseOn)
            {
                Observable.EveryUpdate()
                    .Take(1)
                    .Delay(TimeSpan.FromMilliseconds(25))
                    .Subscribe(_ => TacticalPauseControl.TogglePause());
            }
        }

        public override ISaveSystem Save()
        {
            return new GeneralSaveSystem(GameTime.Frame, GameTime.UnpausedFrame, TacticalPause.IsPausedSelf);
        }
    }
}