using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNonVRPlayer : NetworkBehaviour
{
	private MouseLook mouseLook;
	
	public override void OnStartLocalPlayer()
	{
		GetComponent<Renderer>().material.color = Color.blue;

		// attach camera to player.. 3rd person view..
		Camera.main.transform.parent = transform;
		Camera.main.transform.localPosition = new Vector3(0, 1.33f, -0.69f);
		Camera.main.transform.localRotation = Quaternion.Euler(6.31f, 0, 0);

		mouseLook = new MouseLook();
		mouseLook.Init(transform, Camera.main.transform);
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

		mouseLook.LookRotation(transform, Camera.main.transform);

		transform.rotation = Camera.main.transform.rotation;

	}
}
