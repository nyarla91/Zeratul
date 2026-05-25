using System;
using Newtonsoft.Json;

namespace Save.Data
{
    [Serializable]
    public class GeneralSaveSystem : ISaveSystem
    {
        public static string LoadKey => "general";
        public string SaveKey => LoadKey;

        public int scenarioId;
        public int gameTimeFrame;
        public int gameTimeUnpausedFrame;
        public bool isTacticalPauseOn;
        
        public GeneralSaveSystem(int scenarioId, int gameTimeFrame, int gameTimeUnpausedFrame, bool isTacticalPauseOn)
        {
            this.scenarioId = scenarioId;
            this.gameTimeFrame = gameTimeFrame;
            this.gameTimeUnpausedFrame = gameTimeUnpausedFrame;
            this.isTacticalPauseOn = isTacticalPauseOn;
        }
    }
}