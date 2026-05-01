using Gameplay.Player;
using GameState;
using Saving.Data;
using Zenject;

namespace Gameplay.Saving
{
    public class GeneralSavingSystem : SavingSystem<GeneralSaveSystem>
    {
        [Inject] private ScenarioSession ScenarioSession { get; set; }

        protected override string LoadKey => GeneralSaveSystem.LoadKey;
        
        public override void ReproduceFromSaveData(GeneralSaveSystem payload) { }

        public override ISaveSystem Save()
        {
            int id = ScenarioSession.CurrentId;
            return new GeneralSaveSystem(id);
        }
    }
}