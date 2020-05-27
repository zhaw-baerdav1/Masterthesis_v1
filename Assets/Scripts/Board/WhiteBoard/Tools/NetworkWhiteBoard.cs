using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//responsible to represent whiteboard received by network
public class NetworkWhiteBoard : MonoBehaviour
{
	private int textureSize = 612;
	private Texture2D networkTexture;

	private byte[] receivedTextureBytes;
	private Rect receivedRectangle = Rect.zero;

	private CustomVRPlayer ownerCustomVRPlayer;

	//bind events
	private void Awake()
	{
		WhiteBoardEventSystem.OnReceiveTexture += WhiteBoardEventSystem_OnReceiveTexture;
		WhiteBoardEventSystem.OnResetWhiteBoard += WhiteBoardEventSystem_OnResetWhiteBoard;
	}

	//unbind events
	private void OnDestroy()
	{
		WhiteBoardEventSystem.OnReceiveTexture -= WhiteBoardEventSystem_OnReceiveTexture;
		WhiteBoardEventSystem.OnResetWhiteBoard -= WhiteBoardEventSystem_OnResetWhiteBoard;
	}

	//if a new texture from network has been received
	private void WhiteBoardEventSystem_OnReceiveTexture(int connectionId, Rect receivableRectangle, byte[] textureBytes)
	{
		//do not apply if player is owner
		if (ownerCustomVRPlayer != null && connectionId.Equals(ownerCustomVRPlayer.connectionId))
		{
			return;
		}

		receivedTextureBytes = textureBytes;
		receivedRectangle = receivableRectangle;
	}

	// Use this for initialization
	void Start()
	{
		//default texture setup
		networkTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);

		//reset texture
		ResetTexture();

		//apply texture to whiteboard
		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = (Texture)networkTexture;
	}

	//reset texture to transparent
	private void ResetTexture()
	{
		Color fillColor = Color.clear;
		Color[] fillPixels = networkTexture.GetPixels();

		for (int i = 0; i < fillPixels.Length; i++)
		{
			fillPixels[i] = fillColor;
		}

		networkTexture.SetPixels(fillPixels);
		networkTexture.Apply();
	}

	// Update is called once per frame
	void Update()
	{
		//if texture has been received, apply it
		if (receivedTextureBytes != null)
		{
			ApplyNetworkTexture();

			receivedTextureBytes = null;
		}
	}

	//apply texture received from network
	private void ApplyNetworkTexture()
	{
		//identify coordinates of to-be-applied texture
		int x = (int) receivedRectangle.x;
		int y = (int) receivedRectangle.y;
		int width = (int) receivedRectangle.width;
		int height = (int) receivedRectangle.height;

		//load image received
		Texture2D receivedTexture = new Texture2D(width, height);
		receivedTexture.LoadImage(receivedTextureBytes);

		Color[] pix = receivedTexture.GetPixels();

		networkTexture.SetPixels(x, y, width, height, pix);
		networkTexture.Apply();
	}

	//update owner of whiteboard
	public void SetOwnerCustomVRPlayer(CustomVRPlayer customVRPlayer)
	{
		ownerCustomVRPlayer = customVRPlayer;
	}

	//reset whiteboard if triggered
	private void WhiteBoardEventSystem_OnResetWhiteBoard()
	{
		ResetTexture();
	}
}