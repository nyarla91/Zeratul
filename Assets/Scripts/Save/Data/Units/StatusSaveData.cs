using System;

namespace Save.Data.Units
{
    [Serializable]
    public class StatusSaveData
    {
        public string typeName;
        public int instigatorId;
        public int additionFrame;
        public int removalFrame;

        public StatusSaveData(string typeName, int instigatorId, int additionFrame, int removalFrame)
        {
            this.typeName = typeName;
            this.instigatorId = instigatorId;
            this.additionFrame = additionFrame;
            this.removalFrame = removalFrame;
        }
    }
}