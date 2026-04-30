using Gameplay.Player;
using GameState;
using Saving.Data;
using Zenject;

namespace Gameplay.Saving
{
    public class GenericSavingSystem : SavingSystem<GeneralSaveSystem>
    {
        [Inject] private ScenarioSession ScenarioSession { get; set; }
        
        public override void ReproduceFromSaveData(GeneralSaveSystem payload) { }

        public override ISaveSystem Save()
        {
            int id = ScenarioSession.CurrentId;
            return new GeneralSaveSystem(id);
        }
    }
}