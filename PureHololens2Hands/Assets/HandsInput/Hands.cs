using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR;
using System;

public class Hands : MonoBehaviour
{
    public static Hands Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public HandProxy LeftHandProxy => this.leftHandProxy;
    [SerializeField]
    private HandProxy leftHandProxy;
    public HandProxy RightHandProxy => this.rightHandProxy;
    [SerializeField]
    private HandProxy rightHandProxy;

    private void Update()
    {
        UpdateHand(XRNode.LeftHand, leftHandProxy);
        UpdateHand(XRNode.RightHand, rightHandProxy);
    }

    private void UpdateHand(XRNode node, HandProxy proxy)
    {
        List<InputDevice> devices = new List<InputDevice>(); // Gosh I hate this API...
        InputDevices.GetDevicesAtXRNode(node, devices);
        if (devices.Count == 1)
        {
            UpdateHand(devices[0], proxy);
        }
    }

    private void UpdateHand(InputDevice handDevice, HandProxy handProxy)
    {
        UpdatePalm(handDevice, handProxy);
        //UpdateHandJoint(handDevice, handProxy, "BoneThumbMetacarpal",      handProxy.ThumbMetacarpal);
        UpdateHandJoint(handDevice, handProxy, "BoneThumbProximal", handProxy.ThumbMetacarpal);
        UpdateHandJoint(handDevice, handProxy, "BoneThumbIntermediate", handProxy.ThumbProximal);
        UpdateHandJoint(handDevice, handProxy, "BoneThumbDistal", handProxy.ThumbDistal);
        UpdateHandJoint(handDevice, handProxy, "BoneThumbTip", handProxy.ThumbTip);
        UpdateHandJoint(handDevice, handProxy, "BoneIndexMetacarpal", handProxy.IndexMetacarpal);
        UpdateHandJoint(handDevice, handProxy, "BoneIndexProximal", handProxy.IndexProximal);
        UpdateHandJoint(handDevice, handProxy, "BoneIndexIntermediate", handProxy.IndexIntermediate);
        UpdateHandJoint(handDevice, handProxy, "BoneIndexDistal", handProxy.IndexDistal);
        UpdateHandJoint(handDevice, handProxy, "BoneIndexTip", handProxy.IndexTip);
        UpdateHandJoint(handDevice, handProxy, "BoneMiddleMetacarpal", handProxy.MiddleMetacarpal);
        UpdateHandJoint(handDevice, handProxy, "BoneMiddleProximal", handProxy.MiddleProximal);
        UpdateHandJoint(handDevice, handProxy, "BoneMiddleIntermediate", handProxy.MiddleIntermediate);
        UpdateHandJoint(handDevice, handProxy, "BoneMiddleDistal", handProxy.MiddleDistal);
        UpdateHandJoint(handDevice, handProxy, "BoneMiddleTip", handProxy.MiddleTip);
        UpdateHandJoint(handDevice, handProxy, "BoneRingMetacarpal", handProxy.RingMetacarpal);
        UpdateHandJoint(handDevice, handProxy, "BoneRingProximal", handProxy.RingProximal);
        UpdateHandJoint(handDevice, handProxy, "BoneRingIntermediate", handProxy.RingIntermediate);
        UpdateHandJoint(handDevice, handProxy, "BoneRingDistal", handProxy.RingDistal);
        UpdateHandJoint(handDevice, handProxy, "BoneRingTip", handProxy.RingTip);
        UpdateHandJoint(handDevice, handProxy, "BonePinkyMetacarpal", handProxy.LittleMetacarpal);
        UpdateHandJoint(handDevice, handProxy, "BonePinkyProximal", handProxy.LittleProximal);
        UpdateHandJoint(handDevice, handProxy, "BonePinkyIntermediate", handProxy.LittleIntermediate);
        UpdateHandJoint(handDevice, handProxy, "BonePinkyDistal", handProxy.LittleDistal);
        UpdateHandJoint(handDevice, handProxy, "BonePinkyTip", handProxy.LittleTip);
    }

    private void UpdatePalm(InputDevice handDevice, HandProxy handProxy)
    {
        InputFeatureUsage<Quaternion> rotUsage = new InputFeatureUsage<Quaternion>("DeviceRotation");
        Quaternion handRot;
        if (handDevice.TryGetFeatureValue(rotUsage, out handRot))
        {
            handProxy.Wrist.rotation = handRot * Reorientation(handProxy);
            handProxy.Wrist.rotation *= Quaternion.Euler(handProxy.WristRotationOffset);
        }

        InputFeatureUsage<Vector3> posUsage = new InputFeatureUsage<Vector3>("DevicePosition");
        Vector3 handPos;
        if (handDevice.TryGetFeatureValue(posUsage, out handPos))
        {
            handProxy.Wrist.position = handPos;
            Vector3 offset = handProxy.Wrist.rotation * handProxy.WristPositionOffset;
            handProxy.Wrist.localPosition += offset;
        }
    }

    private void UpdateHandJoint(InputDevice handDevice, HandProxy hand, string name, Transform proxy)
    {
        if (proxy == null)
            return;

        InputFeatureUsage<Bone> usage = new InputFeatureUsage<Bone>(name);
        Bone bone;
        if (handDevice.TryGetFeatureValue(usage, out bone))
        {
            Vector3 jointPos;
            if (bone.TryGetPosition(out jointPos))
                proxy.position = jointPos;
            Quaternion jointRot;
            if (bone.TryGetRotation(out jointRot))
                proxy.rotation = jointRot * Reorientation(hand);
        }
    }

    private Quaternion Reorientation(HandProxy hand)
    {
        return Quaternion.Inverse(Quaternion.LookRotation(hand.ModelFingerPointing, -hand.ModelPalmFacing));
    }
}