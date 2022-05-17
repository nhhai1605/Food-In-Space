using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MovementManager : MonoBehaviour
{
    private GameObject VROrigin;
    //private XRDeviceSimulatorControls controls;
    public Camera VRCamera { get; set; }
    [SerializeField] private float pullingSpeed = 0.02f;
    [SerializeField] private float thrustingSpeed = 0.02f;
    [SerializeField] private float pullingDistance = 0.2f;
    [SerializeField] private float friction = 0.98f;
    [SerializeField] private InputActionReference buttonReference;
    private GameObject pullableObject = null;
    private bool IsThrusting = false;
    Vector3 look = Vector3.zero;
    void Start()
    {
        VROrigin = this.gameObject;
        VRCamera = GetComponentInChildren<Camera>();
    }
    void Awake()
    {
        buttonReference.action.started += Thrust;
        buttonReference.action.canceled += StopThrust;
    }
    void OnDestroy()
    {
        buttonReference.action.started -= Thrust;
        buttonReference.action.canceled -= StopThrust;
    }

    void FixedUpdate()
    {

        if (IsThrusting)
        {
            look = Camera.main.transform.TransformDirection(Vector3.forward);
        }
        else
        {
           look *= friction;
        }
        if(look.magnitude < 0.0001f)
        {
            look = Vector3.zero;
        }
        VROrigin.transform.position += look * thrustingSpeed;

        if (pullableObject != null)
        {
            Vector3 dir = pullableObject.transform.position - VRCamera.transform.position;
            if (pullableObject != null && Vector3.Distance(VRCamera.transform.position, pullableObject.transform.position) > pullingDistance)
            {
                VROrigin.transform.position += dir * pullingSpeed;
            }
        }



    }
    private void Thrust(InputAction.CallbackContext context)
    {
        IsThrusting =true;
    }
    private void StopThrust(InputAction.CallbackContext context)
    {
        IsThrusting = false;
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
