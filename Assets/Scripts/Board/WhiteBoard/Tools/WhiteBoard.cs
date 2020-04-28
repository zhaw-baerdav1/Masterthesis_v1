using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WhiteBoard : MonoBehaviour
{
	private int textureSize = 2048;
	private int penSize = 10;
	private Texture2D texture;
	private Color[] color;

	private bool touching, touchingLast;
	private float posX, posY;
	private float lastX, lastY;

	private CustomVRPlayer ownerCustomVRPlayer;

	private void Awake()
	{
		WhiteBoardEventSystem.OnApplyTexture += WhiteBoardEventSystem_OnApplyTexture;
	}

	private void OnDestroy()
	{
		WhiteBoardEventSystem.OnApplyTexture -= WhiteBoardEventSystem_OnApplyTexture;
	}

	private void WhiteBoardEventSystem_OnApplyTexture(int ownerConnectionId, int x, int y)
	{
		if(ownerCustomVRPlayer != null && ownerCustomVRPlayer.connectionId.Equals(ownerConnectionId))
		{
			return;
		}

		DrawPixels(x, y);
	}

	// Use this for initialization
	void Start()
	{
		// Set whiteboard texture
		texture = new Texture2D(textureSize, textureSize);

		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = (Texture)texture;
	}

	// Update is called once per frame
	void Update()
	{
		// Transform textureCoords into "pixel" values
		int x = (int)(posX * textureSize - (penSize / 2));
		int y = (int)(posY * textureSize - (penSize / 2));

		// Only set the pixels if we were touching last frame
		if (touchingLast)
		{
			DrawPixels(x, y);
		}

		// If currently touching, apply the texture
		if (touching)
		{
			texture.Apply();

			//WhiteBoardEventSystem.ApplyTexture(ownerCustomVRPlayer.connectionId, x, y);
		}

		this.lastX = (float)x;
		this.lastY = (float)y;

		this.touchingLast = this.touching;
	}

	private void DrawPixels(int x, int y)
	{
		// Set base touch pixels
		texture.SetPixels(x, y, penSize, penSize, color);

		// Interpolate pixels from previous touch
		for (float t = 0.01f; t < 1.00f; t += 0.01f)
		{
			int lerpX = (int)Mathf.Lerp(lastX, (float)x, t);
			int lerpY = (int)Mathf.Lerp(lastY, (float)y, t);
			texture.SetPixels(lerpX, lerpY, penSize, penSize, color);
		}
	}

	public void ToggleTouch(bool touching)
	{
		this.touching = touching;
	}

	public void SetTouchPosition(float x, float y)
	{
		this.posX = x;
		this.posY = y;
	}

	public void SetColor(Color color)
	{
		this.color = Enumerable.Repeat<Color>(color, penSize * penSize).ToArray<Color>();
	}

	public void SetOwnerCustomVRPlayer(CustomVRPlayer _ownerCustomVRPlayer)
	{
		ownerCustomVRPlayer = _ownerCustomVRPlayer;
	}
}
