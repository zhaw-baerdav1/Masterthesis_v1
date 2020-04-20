using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomPlayer : NetworkBehaviour
{
	[SyncVar]
	public int connectionId;
	[SyncVar]
	public int spawnOrderNumber;
}
