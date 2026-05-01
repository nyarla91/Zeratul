using Gameplay.Player;
using GameState;
using Saving.Data;
using Zenject;

namespace Gameplay.Saving
{
    public class GeneralSavingSystem : SavingSystem<GeneralSaveSystem>
    {
        [Inject] private ScenarioSession ScenarioSession { get; set; }
        [Inject] private GameTime GameTime { get; set; }

        protected override string LoadKey => GeneralSaveSystem.LoadKey;

        public override void ReproduceFromSaveData(GeneralSaveSystem payload)
        {
            GameTime.ReproduceFromSaveData(payload);
        }

        public override ISaveSystem Save()
        {
            int id = ScenarioSession.CurrentId;
            return new GeneralSaveSystem(id, GameTime.Frame, GameTime.UnpausedFrame);
        }
    }
}