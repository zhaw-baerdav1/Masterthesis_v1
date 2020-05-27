using CrazyMinnow.SALSA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//responsible for applying head rotations on player
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

	//called every frame
	private void Update()
	{
		//do not continue if no authority on object
		if (!hasAuthority) { 
			return;
		}

		//do not continue if not local player
		if (!localPlayerAuthority) { 
			return;
		}

		//do not continue if eyes are not existing or not initialized yet
		if (eyes == null)
		{
			return;
		}

		//get attached camera of player
		Camera _vRCamera = GetComponentInChildren<Camera>();

		//do not continue if camera not found
		if (_vRCamera == null)
		{
			return;
		}

		RaycastHit hit;

		//identify center of camera
		var cameraCenter = _vRCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, _vRCamera.nearClipPlane));

		//true, if an object in the center of the camera has been found
		if (Physics.Raycast(cameraCenter, customVRPlayer.hmdTransform.forward, out hit, Mathf.Infinity))
		{
			Transform hitTransform = hit.transform;
			
			//set the look target of SALSA to this transform
			eyes.lookTarget = hitTransform;

			//update the look target in the network
			lookTargetName = hitTransform.name;
			CmdSetNewLookTarget(lookTargetName);

			//trigger Cube handling
			CubeList.CubeLookAway();

			//do not continue if its not a box collider
			GameObject lookAtGameObject = hit.transform.gameObject;
			BoxCollider lookAtBoxCollider = lookAtGameObject.GetComponent<BoxCollider>();
			if (lookAtBoxCollider == null)
			{
				return;
			}

			//do not continue if its not a cube representation
			CubeRepresentation lookAtCubeRepresentation = lookAtGameObject.GetComponent<CubeRepresentation>();
			if (lookAtCubeRepresentation == null)
			{
				return;
			}

			//the player looks at a cube, trigger event system
			long lookAtCubeDefinitionId = lookAtCubeRepresentation.GetCubeDefinition().id;
			CubeList.CubeLookAt(lookAtCubeDefinitionId);
		}

	}

	//executed on server to update looktarget name of player
	[Command]
	private void CmdSetNewLookTarget(string newLookTargetName)
	{
		lookTargetName = newLookTargetName;
	}

	//triggered on change of look target
	void OnChangeLookTarget(string newLookTargetName)
	{
		//do not change on local player, as player looks already in this direction
		if (isLocalPlayer)
		{
			return;
		}

		lookTargetName = newLookTargetName;

		//find object player is looking
		GameObject lookAtGameObject = GameObject.Find(lookTargetName);
		if (lookAtGameObject == null)
		{
			return;
		}

		//update look target on all clients to the same
		Transform lookAtTransform = lookAtGameObject.GetComponent<Transform>();
		eyes.lookTarget = lookAtTransform;
	}

	
}
