//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Player interface used to query HMD transforms and VR hands
//
//=============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;


//-------------------------------------------------------------------------
// Singleton representing the local VR player/user, with methods for getting
// the player's hands, head, tracking origin, and guesses for various properties.
//-------------------------------------------------------------------------
public class CustomPlayer : MonoBehaviour
	{
		[Tooltip("Virtual transform corresponding to the meatspace tracking origin. Devices are tracked relative to this.")]
		public Transform trackingOriginTransform;

		public Transform eyesOriginTransform;

		[Tooltip("List of possible transforms for the head/HMD, including the no-SteamVR fallback camera.")]
		public Transform vRCameraTransform;

		[Tooltip("List of possible Hands, including no-SteamVR fallback Hands.")]
		public CustomHand[] hands;

		[Tooltip("This action lets you know when the player has placed the headset on their head")]
		public SteamVR_Action_Boolean headsetOnHead = SteamVR_Input.GetBooleanAction("HeadsetOnHead");
	   
		//-------------------------------------------------
		// Singleton instance of the Player. Only one can exist at a time.
		//-------------------------------------------------
		private static CustomPlayer _instance;
		public static CustomPlayer instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<CustomPlayer>();
				}
				return _instance;
			}
		}


		//-------------------------------------------------
		// Get the number of active Hands.
		//-------------------------------------------------
		public int handCount
		{
			get
			{
				int count = 0;
				for (int i = 0; i < hands.Length; i++)
				{
					if (hands[i].gameObject.activeInHierarchy)
					{
						count++;
					}
				}
				return count;
			}
		}


		//-------------------------------------------------
		// Get the i-th active Hand.
		//
		// i - Zero-based index of the active Hand to get
		//-------------------------------------------------
		public CustomHand GetHand(int i)
		{
			for (int j = 0; j < hands.Length; j++)
			{
				if (!hands[j].gameObject.activeInHierarchy)
				{
					continue;
				}

				if (i > 0)
				{
					i--;
					continue;
				}

				return hands[j];
			}

			return null;
		}


		//-------------------------------------------------
		public CustomHand leftHand
		{
			get
			{
				for (int j = 0; j < hands.Length; j++)
				{
					if (!hands[j].gameObject.activeInHierarchy)
					{
						continue;
					}

					if (hands[j].handType != SteamVR_Input_Sources.LeftHand)
					{
						continue;
					}

					return hands[j];
				}

				return null;
			}
		}


		//-------------------------------------------------
		public CustomHand rightHand
		{
			get
			{
				for (int j = 0; j < hands.Length; j++)
				{
					if (!hands[j].gameObject.activeInHierarchy)
					{
						continue;
					}

					if (hands[j].handType != SteamVR_Input_Sources.RightHand)
					{
						continue;
					}

					return hands[j];
				}

				return null;
			}
		}

		//-------------------------------------------------
		// Get Player scale. Assumes it is scaled equally on all axes.
		//-------------------------------------------------

		public float scale
		{
			get
			{
				return transform.lossyScale.x;
			}
		}


		//-------------------------------------------------
		// Get the HMD transform. This might return the fallback camera transform if SteamVR is unavailable or disabled.
		//-------------------------------------------------
		public Transform hmdTransform
		{
			get
			{
				if (vRCameraTransform != null)
				{
					return vRCameraTransform;
				}
				return null;
			}
		}


		//-------------------------------------------------
		// Height of the eyes above the ground - useful for estimating player height.
		//-------------------------------------------------
		public float eyeHeight
		{
			get
			{
				Transform hmd = hmdTransform;
				if (hmd)
				{
					Vector3 eyeOffset = Vector3.Project(hmd.position - eyesOriginTransform.position, eyesOriginTransform.up);
					return eyeOffset.magnitude / eyesOriginTransform.lossyScale.x;
				}
				return 0.0f;
			}
		}


		//-------------------------------------------------
		// Guess for the world-space position of the player's feet, directly beneath the HMD.
		//-------------------------------------------------
		public Vector3 feetPositionGuess
		{
			get
			{
				Transform hmd = hmdTransform;
				if (hmd)
				{
					return trackingOriginTransform.position + Vector3.ProjectOnPlane(hmd.position - trackingOriginTransform.position, trackingOriginTransform.up);
				}
				return trackingOriginTransform.position;
			}
		}


		//-------------------------------------------------
		// Guess for the world-space direction of the player's hips/torso. This is effectively just the gaze direction projected onto the floor plane.
		//-------------------------------------------------
		public Vector3 bodyDirectionGuess
		{
			get
			{
				Transform hmd = hmdTransform;
				if (hmd)
				{
					Vector3 direction = Vector3.ProjectOnPlane(hmd.forward, trackingOriginTransform.up);
					if (Vector3.Dot(hmd.up, trackingOriginTransform.up) < 0.0f)
					{
						// The HMD is upside-down. Either
						// -The player is bending over backwards
						// -The player is bent over looking through their legs
						direction = -direction;
					}
					return direction;
				}
				return trackingOriginTransform.forward;
			}
		}


		//-------------------------------------------------
		private void Awake()
		{
			if (trackingOriginTransform == null)
			{
				trackingOriginTransform = this.transform;
			}
		}


		//-------------------------------------------------
		private IEnumerator Start()
		{
			_instance = this;

			UnityEngine.XR.InputTracking.disablePositionalTracking = false;

			while (SteamVR.initializedState == SteamVR.InitializedStates.None || SteamVR.initializedState == SteamVR.InitializedStates.Initializing)
				yield return null;
		}

		protected virtual void Update()
		{
			if (SteamVR.initializedState != SteamVR.InitializedStates.InitializeSuccess)
				return;

			if (headsetOnHead != null)
			{
				if (headsetOnHead.GetStateDown(SteamVR_Input_Sources.Head))
				{
					Debug.Log("<b>SteamVR Interaction System</b> Headset placed on head");
				}
				else if (headsetOnHead.GetStateUp(SteamVR_Input_Sources.Head))
				{
					Debug.Log("<b>SteamVR Interaction System</b> Headset removed");
				}
			}
		}

		
	}

