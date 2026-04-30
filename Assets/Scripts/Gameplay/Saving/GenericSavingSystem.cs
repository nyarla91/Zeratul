using Gameplay.Player;
using GameState;
using Saving.Data;
using Zenject;

namespace Gameplay.Saving
{
    public class GenericSavingSystem : SavingSystem<GenericSaveSystem>
    {
        [Inject] private ScenarioSession ScenarioSession { get; set; }
        
        public override void ReproduceSavedSystem(GenericSaveSystem payload) { }

        public override ISaveSystem SaveSystem()
        {
            int id = ScenarioSession.CurrentId;
            return new GenericSaveSystem(id);
        }
    }
}