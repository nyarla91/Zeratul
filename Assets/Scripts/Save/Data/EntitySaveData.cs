using System;
using Extentions;

namespace Save.Data
{
	[Serializable]
    public class EntitySaveData
    {
	    public string prefabName;
	    public SerializableVector2 position;
	    public int instigatorId;
	    public Owner owner;
	    public int duration;

	    public EntitySaveData(string prefabName, SerializableVector2 position, int instigatorId, Owner owner, int duration)
	    {
		    this.prefabName = prefabName;
		    this.position = position;
		    this.instigatorId = instigatorId;
		    this.owner = owner;
		    this.duration = duration;
	    }
    }
}