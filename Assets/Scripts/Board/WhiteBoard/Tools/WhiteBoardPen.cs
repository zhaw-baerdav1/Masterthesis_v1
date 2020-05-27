using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//responsible for activities with whiteboard pen
public class WhiteBoardPen : MonoBehaviour
{
	[SerializeField]
	public WhiteBoard whiteBoard;
	[SerializeField]
	public NetworkWhiteBoard networkWhiteBoard;

	private GameObject tip;
	private RaycastHit touch;
	private Quaternion lastAngle;
	private bool lastTouch;
	
	[SerializeField]
	public List<Color> colorList = new List<Color>();
	private int currentColorIndex = 0;

	private Vector3 oldPosition;
	private Quaternion oldRotation;

	private CustomVRPlayer ownerCustomVRPlayer;

	// Use this for initialization
	void Start()
	{
		//store initial position
		oldPosition = transform.position;
		oldRotation = transform.rotation;

		//identify tip of pen
		tip = transform.Find("Tip").gameObject;

		//apply default color
		ApplyTipColor(currentColorIndex);
	}

	//bind events
	public void Awake()
	{
		ColorList.OnSwitchColorLeft += ColorList_OnSwitchColorLeft;
		ColorList.OnSwitchColorRight += ColorList_OnSwitchColorRight;

		WhiteBoardEventSystem.OnResetPens += WhiteBoardEventSystem_OnResetPens;
	}

	//unbind events
	private void OnDestroy()
	{
		ColorList.OnSwitchColorLeft -= ColorList_OnSwitchColorLeft;
		ColorList.OnSwitchColorRight -= ColorList_OnSwitchColorRight;

		WhiteBoardEventSystem.OnResetPens -= WhiteBoardEventSystem_OnResetPens;
	}

	//set owner of pen
	public void SetOwnerCustomVRPlayer(CustomVRPlayer customVRPlayer)
	{
		//only if owner is identified
		if(ownerCustomVRPlayer != null)
		{
			return;
		}

		ownerCustomVRPlayer = customVRPlayer;

		//update respective whiteboards
		whiteBoard.SetOwnerCustomVRPlayer(ownerCustomVRPlayer);
		networkWhiteBoard.SetOwnerCustomVRPlayer(ownerCustomVRPlayer);
	}

	//switch color if event to switch right has been received
	private void ColorList_OnSwitchColorRight()
	{
		if (currentColorIndex == (colorList.Count - 1)){
			ApplyTipColor(0);
			return;
		}
		ApplyTipColor(currentColorIndex + 1);
	}

	//switch color if event to switch left has been received
	private void ColorList_OnSwitchColorLeft()
	{
		if (currentColorIndex == 0)
		{
			ApplyTipColor(colorList.Count - 1);
			return;
		}
		ApplyTipColor(currentColorIndex - 1);
	}

	//update tip of pen to new material
	private void ApplyTipColor(int colorIndex)
	{
		currentColorIndex = colorIndex;

		//ensure its not transparent
		Color color = colorList[currentColorIndex];
		color.a = 1;
		whiteBoard.SetColor(color);

		//ensure standard shader
		Material material = new Material(Shader.Find("Standard"));
		material.color = color;

		//update material
		MeshRenderer meshRenderer = tip.transform.Find("TipColor").GetComponent<MeshRenderer>();
		meshRenderer.material = material;		
	}

	// Update is called once per frame
	void Update()
	{
		float tipHeight = tip.transform.localScale.y;
		Vector3 tipPosition = tip.transform.position;

		// Check for a Raycast from the tip of the pen
		if (Physics.Raycast(tipPosition, transform.up, out touch, tipHeight))
		{			
			if (!touch.transform.gameObject.Equals(whiteBoard.gameObject))
			{
				return;
			}

			// Set whiteboard parameters
			whiteBoard.SetTouchPosition(touch.textureCoord.x, touch.textureCoord.y);
			whiteBoard.ToggleTouch(true);

			// If we started touching, get the current angle of the pen
			if (lastTouch == false)
			{
				lastTouch = true;
				lastAngle = transform.rotation;
			}
		}
		else
		{
			whiteBoard.ToggleTouch(false);
			lastTouch = false;
		}

		// Lock the rotation of the pen if "touching"
		if (lastTouch)
		{
			transform.rotation = lastAngle;
		}
	}

	//set pen to inital position if triggered
	private void WhiteBoardEventSystem_OnResetPens()
	{
		transform.position = oldPosition;
		transform.rotation = oldRotation;
	}
}
