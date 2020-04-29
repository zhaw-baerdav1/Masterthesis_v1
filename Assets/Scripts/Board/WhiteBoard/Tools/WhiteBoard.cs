using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WhiteBoard : MonoBehaviour
{
	private int textureSize = 480;
	private int penSize = 1;
	private Texture2D texture;
	private Color[] color;
	private Texture2D networkTexture;

	private bool touching, touchingLast;
	private float posX, posY;
	private float lastX, lastY;

	private bool shouldSyncWithNetwork = false;
	private bool sendableRectangleIdentified = false;
	private const float pollTimer = 5;
	
	private int minX;
	private int minY;
	private int maxX;
	private int maxY;

	private CustomVRPlayer ownerCustomVRPlayer;

	private void Awake()
	{
		WhiteBoardEventSystem.OnApplyTexture += WhiteBoardEventSystem_OnApplyTexture;

		shouldSyncWithNetwork = true;
	}

	private void OnDestroy()
	{
		WhiteBoardEventSystem.OnApplyTexture -= WhiteBoardEventSystem_OnApplyTexture;

		shouldSyncWithNetwork = false;
	}

	private void WhiteBoardEventSystem_OnApplyTexture(int connectionId, int startX, int startY, int width, int height, byte[] textureBytes)
	{
		if (ownerCustomVRPlayer != null && connectionId.Equals(ownerCustomVRPlayer.connectionId))
		{
			return;
		}

		Texture2D receivedTexture = new Texture2D(width, height);
		receivedTexture.LoadImage(textureBytes);
		
		Color[] pix = receivedTexture.GetPixels();

		networkTexture.SetPixels(startX, startY, width, height, pix);
		networkTexture.Apply();
	}

	// Use this for initialization
	void Start()
	{
		// Set whiteboard texture
		texture = new Texture2D(textureSize, textureSize);
		networkTexture = new Texture2D(textureSize, textureSize);

		Renderer renderer = GetComponent<Renderer>();
		renderer.materials[0].mainTexture = (Texture)texture;
		renderer.materials[1].mainTexture = (Texture)networkTexture;
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
		}

		this.lastX = (float)x;
		this.lastY = (float)y;

		this.touchingLast = this.touching;
	}

	public void SetOwnerCustomVRPlayer(CustomVRPlayer customVRPlayer)
	{
		ownerCustomVRPlayer = customVRPlayer;
		
		StartCoroutine(SyncTexture());
	}

	private void DrawPixels(int x, int y)
	{
		// Set base touch pixels
		texture.SetPixels(x, y, penSize, penSize, color);
		DetectUsedRectangle(x, y);

		// Interpolate pixels from previous touch
		for (float t = 0.01f; t < 1.00f; t += 0.01f)
		{
			int lerpX = (int)Mathf.Lerp(lastX, (float)x, t);
			int lerpY = (int)Mathf.Lerp(lastY, (float)y, t);
			texture.SetPixels(lerpX, lerpY, penSize, penSize, color);
			DetectUsedRectangle(lerpX, lerpY);
		}
	}

	private void DetectUsedRectangle(int x, int y)
	{
		minX = Mathf.Min(x, minX);
		minY = Mathf.Min(y, minY);
		maxX = Mathf.Max(x, maxX);
		maxY = Mathf.Max(y, maxY);

		sendableRectangleIdentified = true;
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
	
	private IEnumerator SyncTexture()
	{
		float timeCheck = Time.time;
		while (shouldSyncWithNetwork)
		{
			if(Time.time - timeCheck > pollTimer && sendableRectangleIdentified)
			{
				int width = maxX - minX;
				int height = maxY - minY;

				Color[] pix = texture.GetPixels(minX, minY, width, height);

				Texture2D sendableTexture = new Texture2D(width, height);
				sendableTexture.SetPixels(pix);
				sendableTexture.Apply();

				WhiteBoardEventSystem.ApplyTexture(ownerCustomVRPlayer.connectionId, minX, minY, width, height, sendableTexture.EncodeToPNG());

				timeCheck = Time.time;
				sendableRectangleIdentified = false;
			}

			yield return null;
		}
	}
}
