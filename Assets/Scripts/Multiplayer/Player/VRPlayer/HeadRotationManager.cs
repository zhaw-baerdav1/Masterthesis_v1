using CrazyMinnow.SALSA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeadRotationManager : NetworkBehaviour
{
	private Eyes eyes;
	private CustomVRPlayer customVRPlayer;

	[SyncVar(hook = "OnChangeLookTarget")]
	public string lookTargetName;

	private void Start()
	{
		eyes = GetComponent<Eyes>();
		customVRPlayer = GetComponent<CustomVRPlayer>();
	}

	private void Update()
	{
		if (!hasAuthority) { 
			return;
		}

		if (!localPlayerAuthority) { 
			return;
		}

		//if (NetworkServer.active) { 
		//	return;
		//}

		if (eyes == null)
		{
			return;
		}

		Camera _vRCamera = GetComponentInChildren<Camera>();
		if (_vRCamera == null)
		{
			return;
		}

		RaycastHit hit;
		var cameraCenter = _vRCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, _vRCamera.nearClipPlane));
		if (Physics.Raycast(cameraCenter, customVRPlayer.hmdTransform.forward, out hit, Mathf.Infinity))
		{
			Transform hitTransform = hit.transform;
			Transform headOriginTransform = customVRPlayer.headOriginTransform;

			if (headOriginTransform != null && headOriginTransform.Equals(hitTransform.parent.transform))
			{
				return;
			}

			eyes.lookTarget = hitTransform;

			lookTargetName = hitTransform.name;
			CmdSetNewLookTarget(lookTargetName);

			//if (Time.time - m_LastClientSendTime > GetNetworkSendInterval())
			//{
			//	SendTransform();
			//	m_LastClientSendTime = Time.time;
			//}
		}

	}

	[Command]
	private void CmdSetNewLookTarget(string newLookTargetName)
	{
		lookTargetName = newLookTargetName;
	}

	void OnChangeLookTarget(string newLookTargetName)
	{
		if (isLocalPlayer)
		{
			return;
		}

		lookTargetName = newLookTargetName;

		GameObject lookAtGameObject = GameObject.Find(lookTargetName);
		if (lookAtGameObject == null)
		{
			return;
		}

		Transform lookAtTransform = lookAtGameObject.GetComponent<Transform>();
		eyes.lookTarget = lookAtTransform;
	}

	//const short SkeletonMsg = 199;

	//[SerializeField]
	//float m_SendInterval = 0.1f;

	//[SerializeField]
	//int m_NetworkChannel = Channels.DefaultUnreliable;

	//float m_LastClientSyncTime; // last time client received a sync from server
	//float m_LastClientSendTime; // last time client send a sync to server

	//NetworkWriter m_LocalTransformWriter;

	//public float sendInterval { get { return m_SendInterval; } set { m_SendInterval = value; } }

	//// runtime data
	//public float lastSyncTime { get { return m_LastClientSyncTime; } }

	//void OnValidate()
	//{
	//	if (m_SendInterval < 0)
	//	{
	//		m_SendInterval = 0;
	//	}
	//}

	//void Awake()
	//{
	//	// cache these to avoid per-frame allocations.
	//	if (localPlayerAuthority)
	//	{
	//		m_LocalTransformWriter = new NetworkWriter();
	//	}

	//	NetworkServer.RegisterHandler(SkeletonMsg, HandleSkeleton);
	//}

	//public override bool OnSerialize(NetworkWriter writer, bool initialState)
	//{
	//	if (initialState)
	//	{
	//		// dont send in initial data. size is likely too large for default channel
	//		return true;
	//	}

	//	if (syncVarDirtyBits == 0)
	//	{
	//		writer.WritePackedUInt32(0);
	//		return false;
	//	}

	//	// dirty bits
	//	writer.WritePackedUInt32(1);

	//	SerializeModeTransform(writer);
	//	return true;
	//}

	//void SerializeModeTransform(NetworkWriter writer)
	//{
	//	writer.Write(lookTargetName);
	//}


	//public override void OnDeserialize(NetworkReader reader, bool initialState)
	//{
	//	if (initialState)
	//		return;

	//	if (isServer && NetworkServer.localClientActive)
	//		return;

	//	if (reader.ReadPackedUInt32() == 0)
	//		return;

	//	UnserializeModeTransform(reader, initialState);

	//	m_LastClientSyncTime = Time.time;
	//}

	//void UnserializeModeTransform(NetworkReader reader, bool initialState)
	//{
	//	if (hasAuthority)
	//	{
	//		// this component must read the data that the server wrote, even if it ignores it.
	//		// otherwise the NetworkReader stream will still contain that data for the next component.

	//		reader.ReadString();
	//		return;
	//	}

	//	lookTargetName = reader.ReadString();
	//}

	//void FixedUpdate()
	//{
	//	if (isServer)
	//	{
	//		FixedUpdateServer();
	//	}
	//	if (isClient)
	//	{
	//		FixedUpdateClient();
	//	}
	//}

	//void FixedUpdateServer()
	//{
	//	if (syncVarDirtyBits != 0)
	//		return;

	//	// dont run if network isn't active
	//	if (!NetworkServer.active)
	//		return;

	//	// dont run if we haven't been spawned yet
	//	if (!isServer)
	//		return;

	//	// dont' auto-dirty if no send interval
	//	if (GetNetworkSendInterval() == 0)
	//		return;

	//	// This will cause transform to be sent
	//	SetDirtyBit(1);
	//}

	//void FixedUpdateClient()
	//{
	//	// dont run if we haven't received any sync data
	//	if (m_LastClientSyncTime == 0)
	//		return;

	//	// dont run if network isn't active
	//	if (!NetworkServer.active && !NetworkClient.active)
	//		return;

	//	// dont run if we haven't been spawned yet
	//	if (!isServer && !isClient)
	//		return;

	//	// dont run if not expecting continuous updates
	//	if (GetNetworkSendInterval() == 0)
	//		return;

	//	// dont run this if this client has authority over this player object
	//	if (hasAuthority)
	//		return;

	//	// interpolate on client
	//	GameObject lookAtGameObject = GameObject.Find(lookTargetName);
	//	if ( lookTargetName == null)
	//	{
	//		return;
	//	}

	//	Transform lookAtTransform = lookAtGameObject.GetComponent<Transform>();
	//	eyes.lookTarget = lookAtTransform;
	//}


	//[Client]
	//void SendTransform()
	//{
	//	if (ClientScene.readyConnection == null)
	//	{
	//		return;
	//	}

	//	m_LocalTransformWriter.StartMessage(SkeletonMsg);
	//	m_LocalTransformWriter.Write(netId);
	//	SerializeModeTransform(m_LocalTransformWriter);

	//	m_LocalTransformWriter.FinishMessage();

	//	ClientScene.readyConnection.SendWriter(m_LocalTransformWriter, GetNetworkChannel());
	//}

	//static internal void HandleSkeleton(NetworkMessage netMsg)
	//{
	//	NetworkInstanceId netId = netMsg.reader.ReadNetworkId();

	//	GameObject foundObj = NetworkServer.FindLocalObject(netId);
	//	if (foundObj == null)
	//	{
	//		if (LogFilter.logError) { Debug.LogError("NetworkSkeleton no gameObject"); }
	//		return;
	//	}

	//	HeadRotationManager headRotationManager = foundObj.GetComponent<HeadRotationManager>();
	//	if (headRotationManager == null)
	//	{
	//		if (LogFilter.logError) { Debug.LogError("NetworkSkeleton null target"); }
	//		return;
	//	}

	//	if (!netMsg.conn.clientOwnedObjects.Contains(netId))
	//	{
	//		if (LogFilter.logWarn) { Debug.LogWarning("NetworkSkeleton netId:" + netId + " is not for a valid player"); }
	//		return;
	//	}

	//	headRotationManager.UnserializeModeTransform(netMsg.reader, false);
	//	headRotationManager.m_LastClientSyncTime = Time.time;
	//}

	//public override int GetNetworkChannel()
	//{
	//	return m_NetworkChannel;
	//}
	//public override float GetNetworkSendInterval()
	//{
	//	return m_SendInterval;
	//}
}
