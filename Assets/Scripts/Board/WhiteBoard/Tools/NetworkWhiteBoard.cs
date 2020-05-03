using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkWhiteBoard : MonoBehaviour
{
	private int textureSize = 612;
	private Texture2D networkTexture;

	private byte[] receivedTextureBytes;
	private Rect receivedRectangle = Rect.zero;

	private CustomVRPlayer ownerCustomVRPlayer;

	private void Awake()
	{
		WhiteBoardEventSystem.OnReceiveTexture += WhiteBoardEventSystem_OnReceiveTexture;
	}

	private void OnDestroy()
	{
		WhiteBoardEventSystem.OnReceiveTexture -= WhiteBoardEventSystem_OnReceiveTexture;
	}

	private void WhiteBoardEventSystem_OnReceiveTexture(int connectionId, Rect receivableRectangle, byte[] textureBytes)
	{
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
		networkTexture = new Texture2D(textureSize, textureSize);

		Color fillColor = Color.clear;
		Color[] fillPixels = networkTexture.GetPixels();

		for (int i = 0; i < fillPixels.Length; i++)
		{
			fillPixels[i] = fillColor;
		}

		networkTexture.SetPixels(fillPixels);
		networkTexture.Apply();

		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = (Texture)networkTexture;
	}

	// Update is called once per frame
	void Update()
	{
		if (receivedTextureBytes != null)
		{
			ApplyNetworkTexture();

			receivedTextureBytes = null;
		}
	}

	private void ApplyNetworkTexture()
	{
		int x = (int) receivedRectangle.x;
		int y = (int) receivedRectangle.y;
		int width = (int) receivedRectangle.width;
		int height = (int) receivedRectangle.height;

		Texture2D receivedTexture = new Texture2D(width, height);
		receivedTexture.LoadImage(receivedTextureBytes);

		Color[] pix = receivedTexture.GetPixels();

		networkTexture.SetPixels(x, y, width, height, pix);
		networkTexture.Apply();
	}

	public void SetOwnerCustomVRPlayer(CustomVRPlayer customVRPlayer)
	{
		ownerCustomVRPlayer = customVRPlayer;
	}
}