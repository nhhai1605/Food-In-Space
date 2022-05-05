using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
public class MovementManager : MonoBehaviour
{
    private GameObject VROrigin;
    public Camera VRCamera { get; set; }
    [SerializeField] private float pullingSpeed = 0.02f;
    [SerializeField] private float thrustingSpeed = 0.02f;
    [SerializeField] private float pullingDistance = 0.2f;
    private GameObject pullableObject = null;
    void Start()
    {
        VROrigin = this.gameObject;
        VRCamera = GetComponentInChildren<Camera>();
    }
    void FixedUpdate()
    {
        if(pullableObject != null)
        {
            Vector3 dir = pullableObject.transform.position - VRCamera.transform.position;
            if (pullableObject != null && Vector3.Distance(VRCamera.transform.position, pullableObject.transform.position) > pullingDistance)
            {
                VROrigin.transform.position += dir * pullingSpeed;
            }
        }

        //Vector3 look = Camera.main.transform.TransformDirection(Vector3.forward);
        //VROrigin.transform.position += look * thrustingSpeed;

    }

    public void Pull(GameObject pullableObject)
    {
        this.pullableObject = pullableObject;
        if (VROrigin.name == "MockOrigin")
        {
            VRCamera.GetComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>().trackingType = UnityEngine.InputSystem.XR.TrackedPoseDriver.TrackingType.RotationOnly;
        }
        else if (VROrigin.name == "OculusOrigin")
        {
            VRCamera.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>().trackingType = UnityEngine.SpatialTracking.TrackedPoseDriver.TrackingType.RotationOnly;
        }
        else
        {
            Debug.LogError("Cannot find the VROrigin");
        }
    }

    public void Release()
    {
        this.pullableObject = null;
        if (VROrigin.name == "MockOrigin")
        {
            VRCamera.GetComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>().trackingType = UnityEngine.InputSystem.XR.TrackedPoseDriver.TrackingType.RotationAndPosition;
        }
        else if (VROrigin.name == "OculusOrigin")
        {
            VRCamera.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>().trackingType = UnityEngine.SpatialTracking.TrackedPoseDriver.TrackingType.RotationAndPosition;
        }
        else
        {
            Debug.LogError("Cannot find the VROrigin");
        }
    }
}
