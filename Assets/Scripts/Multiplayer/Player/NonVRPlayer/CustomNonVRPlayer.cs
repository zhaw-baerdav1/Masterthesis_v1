using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.XR;

public class CustomNonVRPlayer : CustomPlayer
{
	private Camera staticCamera;
	private MouseLook mouseLook;

	public override void OnStartLocalPlayer()
	{
		staticCamera = GetComponentInChildren<Camera>();

		// attach camera to player.. 3rd person view..
		staticCamera.transform.parent = transform;
		staticCamera.transform.localPosition = new Vector3(0, 1.33f, -0.69f);
		staticCamera.transform.localRotation = Quaternion.Euler(6.31f, 0, 0);

		XRSettings.enabled = false;
		XRSettings.LoadDeviceByName("");

		mouseLook = new MouseLook();
		mouseLook.Init(transform, staticCamera.transform);

		base.OnStartLocalPlayer();
	}

	void Update()
	{

		if (!isLocalPlayer)
		{
			return;
		}


		// non vr player input here
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 3.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		transform.Translate(x, 0, z);

		if ( mouseLook != null) { 
			mouseLook.LookRotation(transform, staticCamera.transform);
		}

		transform.rotation = staticCamera.transform.rotation;

	}
}
