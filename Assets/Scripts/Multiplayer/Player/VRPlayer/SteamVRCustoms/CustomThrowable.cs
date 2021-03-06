﻿//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Basic throwable object
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Valve.VR.InteractionSystem;

//-------------------------------------------------------------------------
[RequireComponent( typeof( CustomInteractable ) )]
[RequireComponent( typeof( Rigidbody ) )]
public class CustomThrowable : MonoBehaviour
{
	[EnumFlags]
	[Tooltip( "The flags used to attach this object to the hand." )]
	public CustomHand.AttachmentFlags attachmentFlags = CustomHand.AttachmentFlags.ParentToHand | CustomHand.AttachmentFlags.DetachFromOtherHand | CustomHand.AttachmentFlags.TurnOnKinematic;

    [Tooltip("The local point which acts as a positional and rotational offset to use while held")]
    public Transform attachmentOffset;

	[Tooltip( "How fast must this object be moving to attach due to a trigger hold instead of a trigger press? (-1 to disable)" )]
    public float catchingSpeedThreshold = -1;

    public ReleaseStyle releaseVelocityStyle = ReleaseStyle.GetFromHand;

    [Tooltip("The time offset used when releasing the object with the RawFromHand option")]
    public float releaseVelocityTimeOffset = -0.011f;

    public float scaleReleaseVelocity = 1.1f;

    [Tooltip("The release velocity magnitude representing the end of the scale release velocity curve. (-1 to disable)")]
    public float scaleReleaseVelocityThreshold = -1.0f;
    [Tooltip("Use this curve to ease into the scaled release velocity based on the magnitude of the measured release velocity. This allows greater differentiation between a drop, toss, and throw.")]
    public AnimationCurve scaleReleaseVelocityCurve = AnimationCurve.EaseInOut(0.0f, 0.1f, 1.0f, 1.0f);

    [Tooltip( "When detaching the object, should it return to its original parent?" )]
	public bool restoreOriginalParent = false;



	protected VelocityEstimator velocityEstimator;
    protected bool attached = false;
    protected float attachTime;
    protected Vector3 attachPosition;
    protected Quaternion attachRotation;
    protected Transform attachEaseInTransform;

	public UnityEvent onPickUp;
    public UnityEvent onDetachFromHand;
    public HandEvent onHeldUpdate;


    protected RigidbodyInterpolation hadInterpolation = RigidbodyInterpolation.None;

    protected new Rigidbody rigidbody;

    [HideInInspector]
    public CustomInteractable interactable;


    //-------------------------------------------------
    protected virtual void Awake()
	{
		velocityEstimator = GetComponent<VelocityEstimator>();
        interactable = GetComponent<CustomInteractable>();



        rigidbody = GetComponent<Rigidbody>();
        rigidbody.maxAngularVelocity = 50.0f;


        if(attachmentOffset != null)
        {
            // remove?
            //interactable.handFollowTransform = attachmentOffset;
        }

	}


    //-------------------------------------------------
    protected virtual void OnHandHoverBegin( CustomHand hand )
	{

        // "Catch" the throwable by holding down the interaction button instead of pressing it.
        // Only do this if the throwable is moving faster than the prescribed threshold speed,
        // and if it isn't attached to another hand
        if ( !attached && catchingSpeedThreshold != -1)
        {

            GrabTypes bestGrabType = hand.GetBestGrabbingType();

            if ( bestGrabType != GrabTypes.None )
			{
				
				hand.AttachObject( gameObject, bestGrabType, attachmentFlags );
				
			}
		}
	}


    //-------------------------------------------------
    protected virtual void OnHandHoverEnd(CustomHand hand )
	{

	}


    //-------------------------------------------------
    protected virtual void HandHoverUpdate(CustomHand hand )
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();

        if (startingGrabType != GrabTypes.None)
        {
			hand.AttachObject( gameObject, startingGrabType, attachmentFlags, attachmentOffset );
        }
	}

    //-------------------------------------------------
    protected virtual void OnAttachedToHand(CustomHand hand )
	{
        //Debug.Log("<b>[SteamVR Interaction]</b> Pickup: " + hand.GetGrabStarting().ToString());

        hadInterpolation = this.rigidbody.interpolation;

        attached = true;

		onPickUp.Invoke();

		hand.HoverLock( null );

        rigidbody.interpolation = RigidbodyInterpolation.None;

        if (velocityEstimator != null)
		    velocityEstimator.BeginEstimatingVelocity();

		attachTime = Time.time;
		attachPosition = transform.position;
		attachRotation = transform.rotation;

	}


    //-------------------------------------------------
    protected virtual void OnDetachedFromHand(CustomHand hand)
    {
        attached = false;

        onDetachFromHand.Invoke();

        hand.HoverUnlock(null);

        rigidbody.interpolation = hadInterpolation;

        Vector3 velocity;
        Vector3 angularVelocity;

        GetReleaseVelocities(hand, out velocity, out angularVelocity);

        rigidbody.velocity = velocity;
        rigidbody.angularVelocity = angularVelocity;
    }


    public virtual void GetReleaseVelocities(CustomHand hand, out Vector3 velocity, out Vector3 angularVelocity)
    {
        if (releaseVelocityStyle != ReleaseStyle.NoChange)
            releaseVelocityStyle = ReleaseStyle.ShortEstimation; // only type that works with fallback hand is short estimation.

        switch (releaseVelocityStyle)
        {
            case ReleaseStyle.ShortEstimation:
                if (velocityEstimator != null)
                {
                    velocityEstimator.FinishEstimatingVelocity();
                    velocity = velocityEstimator.GetVelocityEstimate();
                    angularVelocity = velocityEstimator.GetAngularVelocityEstimate();
                }
                else
                {
                    Debug.LogWarning("[SteamVR Interaction System] Throwable: No Velocity Estimator component on object but release style set to short estimation. Please add one or change the release style.");

                    velocity = rigidbody.velocity;
                    angularVelocity = rigidbody.angularVelocity;
                }
                break;
            case ReleaseStyle.AdvancedEstimation:
                hand.GetEstimatedPeakVelocities(out velocity, out angularVelocity);
                break;
            case ReleaseStyle.GetFromHand:
                velocity = hand.GetTrackedObjectVelocity(releaseVelocityTimeOffset);
                angularVelocity = hand.GetTrackedObjectAngularVelocity(releaseVelocityTimeOffset);
                break;
            default:
            case ReleaseStyle.NoChange:
                velocity = rigidbody.velocity;
                angularVelocity = rigidbody.angularVelocity;
                break;
        }

        if (releaseVelocityStyle != ReleaseStyle.NoChange)
        {
                float scaleFactor = 1.0f;
                if (scaleReleaseVelocityThreshold > 0)
                {
                    scaleFactor = Mathf.Clamp01(scaleReleaseVelocityCurve.Evaluate(velocity.magnitude / scaleReleaseVelocityThreshold));
                }

                velocity *= (scaleFactor * scaleReleaseVelocity);
        }
    }

    //-------------------------------------------------
    protected virtual void HandAttachedUpdate(CustomHand hand)
    {


        if (hand.IsGrabEnding(this.gameObject))
        {
            hand.DetachObject(gameObject, restoreOriginalParent);

            // Uncomment to detach ourselves late in the frame.
            // This is so that any vehicles the player is attached to
            // have a chance to finish updating themselves.
            // If we detach now, our position could be behind what it
            // will be at the end of the frame, and the object may appear
            // to teleport behind the hand when the player releases it.
            //StartCoroutine( LateDetach( hand ) );
        }

        if (onHeldUpdate != null)
            onHeldUpdate.Invoke(hand);
    }


    //-------------------------------------------------
    protected virtual IEnumerator LateDetach( CustomHand hand )
	{
		yield return new WaitForEndOfFrame();

		hand.DetachObject( gameObject, restoreOriginalParent );
	}


    //-------------------------------------------------
    protected virtual void OnHandFocusAcquired(CustomHand hand )
	{
		gameObject.SetActive( true );

        if (velocityEstimator != null)
			velocityEstimator.BeginEstimatingVelocity();
	}


    //-------------------------------------------------
    protected virtual void OnHandFocusLost(CustomHand hand )
	{
		gameObject.SetActive( false );

        if (velocityEstimator != null)
            velocityEstimator.FinishEstimatingVelocity();
	}
}

public enum ReleaseStyle
{
    NoChange,
    GetFromHand,
    ShortEstimation,
    AdvancedEstimation,
}