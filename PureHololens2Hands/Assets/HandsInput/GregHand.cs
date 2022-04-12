using Microsoft.MixedReality.OpenXR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GregHand : MonoBehaviour
{
    [SerializeField]
    private bool isLeft;

    [SerializeField]
    private HandProxy handProxy;

    private readonly HandJointLocation[] HandJointLocations = new HandJointLocation[HandTracker.JointCount];
    
    private void Update()
    {
        UpdateHandJoints(isLeft ? HandTracker.Left : HandTracker.Right, FrameTime.OnUpdate);
    }
    
    private void UpdateHandJoints(HandTracker handTracker, FrameTime frameTime)
    {
        if (handTracker.TryLocateHandJoints(frameTime, HandJointLocations))
        {
            //Apply(handProxy.Palm, HandJoint.Palm);
            Apply(handProxy.Wrist, HandJoint.Wrist);
            ApplyWrist(handProxy.ThumbMetacarpal, HandJoint.ThumbMetacarpal);
            Apply(handProxy.ThumbProximal, HandJoint.ThumbProximal);
            Apply(handProxy.ThumbDistal, HandJoint.ThumbDistal);
            Apply(handProxy.ThumbTip, HandJoint.ThumbTip);
            Apply(handProxy.IndexMetacarpal, HandJoint.IndexMetacarpal);
            Apply(handProxy.IndexProximal, HandJoint.IndexProximal);
            Apply(handProxy.IndexIntermediate, HandJoint.IndexIntermediate);
            Apply(handProxy.IndexTip, HandJoint.IndexTip);
            Apply(handProxy.MiddleMetacarpal, HandJoint.MiddleMetacarpal);
            Apply(handProxy.MiddleProximal, HandJoint.MiddleProximal);
            Apply(handProxy.MiddleIntermediate, HandJoint.MiddleIntermediate);
            Apply(handProxy.MiddleDistal, HandJoint.MiddleDistal);
            Apply(handProxy.MiddleTip, HandJoint.MiddleTip);
            Apply(handProxy.RingMetacarpal, HandJoint.RingMetacarpal);
            Apply(handProxy.RingProximal, HandJoint.RingProximal);
            Apply(handProxy.RingIntermediate, HandJoint.RingIntermediate);
            Apply(handProxy.RingDistal, HandJoint.RingDistal);
            Apply(handProxy.RingTip, HandJoint.RingTip);
            Apply(handProxy.LittleMetacarpal, HandJoint.LittleMetacarpal);
            Apply(handProxy.LittleProximal, HandJoint.LittleProximal);
            Apply(handProxy.LittleIntermediate, HandJoint.LittleIntermediate);
            Apply(handProxy.LittleDistal, HandJoint.LittleDistal);
            Apply(handProxy.LittleTip, HandJoint.LittleTip);
        }
        else
        {
            // Disable the hand
        }
    }

    private void ApplyWrist(Transform jointTransform, HandJoint joint)
    {
        if (jointTransform == null)
            return;
        HandJointLocation location = HandJointLocations[(int)joint];
        Quaternion rot = location.Pose.rotation * Reorientation(handProxy);
        rot *= Quaternion.Euler(handProxy.WristRotationOffset);
        Vector3 pos = location.Pose.position;
        pos += handProxy.Wrist.rotation * handProxy.WristPositionOffset;
        jointTransform.SetPositionAndRotation(pos, rot);
    }

    private void Apply(Transform jointTransform, HandJoint joint)
    {
        if (jointTransform == null)
            return;
        HandJointLocation location = HandJointLocations[(int)joint];
        Quaternion rot = location.Pose.rotation * Reorientation(handProxy);
        jointTransform.SetPositionAndRotation(location.Pose.position, rot);
        //jointTransform.localScale = Vector3.one * location.Radius;
    }

    private Quaternion Reorientation(HandProxy hand)
    {
        return Quaternion.Inverse(Quaternion.LookRotation(hand.ModelFingerPointing, -hand.ModelPalmFacing));
    }
}
