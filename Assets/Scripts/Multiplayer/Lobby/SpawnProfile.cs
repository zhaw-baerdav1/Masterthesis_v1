using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnProfile : MessageBase
{
	public int characterId;

	public override void Deserialize(NetworkReader reader)
	{
		characterId = reader.ReadInt32();
	}

	public override void Serialize(NetworkWriter writer)
	{
		writer.Write(characterId);
	}
}
