using Microsoft.MixedReality.OpenXR;
using System.Collections.Generic;
using UnityEngine;

public class OpenXrHands : MonoBehaviour
{
    private Hand leftHand = null;
    private Hand rightHand = null;

    private static readonly HandJointLocation[] HandJointLocations = new HandJointLocation[HandTracker.JointCount];

    private void Start()
    {
        leftHand = new Hand();
        rightHand = new Hand();
    }

    private void Update()
    {
        UpdateHandJoints(HandTracker.Left, leftHand, FrameTime.OnUpdate);
        UpdateHandJoints(HandTracker.Right, rightHand, FrameTime.OnUpdate);
    }

    private static void UpdateHandJoints(HandTracker handTracker, Hand hand, FrameTime frameTime)
    {
        if (handTracker.TryLocateHandJoints(frameTime, HandJointLocations))
        {
            hand?.UpdateHandJoints(HandJointLocations);
        }
        else
        {
            hand?.DisableHandJoints();
        }
    }

    private class Hand
    {
        private readonly GameObject handRoot = null;

        // Visualize hand joints when using OpenXR HandTracker.LocateJoints
        private static readonly HandJoint[] HandJoints = System.Enum.GetValues(typeof(HandJoint)) as HandJoint[];

        private readonly Dictionary<HandJoint, GameObject> handJointGameObjects = new Dictionary<HandJoint, GameObject>();
        public Hand()
        {
            handRoot = new GameObject("HandParent");
        }

        private GameObject InstantiateJointPrefab(HandJoint handJoint)
        {
            GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere); // TODO: Change this to not be a sphere
            Destroy(gameObject.GetComponent<Collider>());
            gameObject.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
            gameObject.transform.parent = handRoot.transform;

            return gameObject;
        }

        public void UpdateHandJoints(HandJointLocation[] locations)
        {
            // If the hand was previously disabled, this is the first new update and it should be re-enabled
            if (!handRoot.activeSelf)
            {
                handRoot.SetActive(true);
            }

            foreach (HandJoint handJoint in HandJoints)
            {
                if (!handJointGameObjects.ContainsKey(handJoint))
                {
                    handJointGameObjects[handJoint] = InstantiateJointPrefab(handJoint);
                }

                GameObject handJointGameObject = handJointGameObjects[handJoint];
                HandJointLocation handJointLocation = locations[(int)handJoint];
                handJointGameObject.transform.SetPositionAndRotation(handJointLocation.Pose.position, handJointLocation.Pose.rotation);
                handJointGameObject.transform.localScale = Vector3.one * handJointLocation.Radius;
            }
        }

        public void DisableHandJoints()
        {
            if (handRoot != null)
            {
                handRoot.SetActive(false);
            }
        }
    }
}