using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WhiteBoard : MonoBehaviour
{
	private int textureSize = 612;
	private int penSize = 2;
	private Texture2D texture;
	private Color[] color;
	private Texture2D networkTexture;

	private bool touching, touchingLast;
	private float posX, posY;
	private float lastX, lastY;

	private bool shouldSyncWithNetwork = false;
	private const float pollTimer = 2;
	private Rect sendableRectangle = Rect.zero;

	private byte[] receivedTextureBytes;
	private Rect receivedRectangle = Rect.zero;

	private CustomVRPlayer ownerCustomVRPlayer;

	private void Awake()
	{
		WhiteBoardEventSystem.OnReceiveTexture += WhiteBoardEventSystem_OnReceiveTexture;

		shouldSyncWithNetwork = true;
	}

	private void OnDestroy()
	{
		WhiteBoardEventSystem.OnReceiveTexture -= WhiteBoardEventSystem_OnReceiveTexture;

		shouldSyncWithNetwork = false;
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

		if (receivedTextureBytes != null)
		{
			ApplyNetworkTexture();

			receivedTextureBytes = null;
		}

		this.lastX = (float)x;
		this.lastY = (float)y;

		this.touchingLast = this.touching;
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
		int xMin = (int) Mathf.Min(x, sendableRectangle.xMin == 0 ? x : sendableRectangle.xMin);
		int yMin = (int) Mathf.Min(y, sendableRectangle.yMin == 0 ? x : sendableRectangle.yMin);
		int xMax = (int) Mathf.Max(x, sendableRectangle.xMax);
		int yMax = (int) Mathf.Max(y, sendableRectangle.yMax);

		sendableRectangle = Rect.MinMaxRect(xMin, yMin, xMax, yMax);
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
			if(Time.time - timeCheck > pollTimer && !sendableRectangle.Equals(Rect.zero))
			{
				Color[] pix = texture.GetPixels((int)sendableRectangle.x, (int)sendableRectangle.y, (int)sendableRectangle.width, (int)sendableRectangle.height);

				Texture2D sendableTexture = new Texture2D((int)sendableRectangle.width, (int)sendableRectangle.height);
				sendableTexture.SetPixels(pix);
				sendableTexture.Apply();

				WhiteBoardEventSystem.SendTexture(ownerCustomVRPlayer.connectionId, sendableRectangle, sendableTexture.EncodeToPNG());

				timeCheck = Time.time;
				sendableRectangle = Rect.zero;
			}

			yield return null;
		}
	}
}
