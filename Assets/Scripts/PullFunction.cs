using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
public class PullFunction : MonoBehaviour
    {
    [SerializeField] GameObject XROrigin;
    private Vector3 target;
    public bool isPulling = false;
    void Start()
    {
        target = XROrigin.transform.position;
    }

    void Update()
    {
        if (isPulling)
        {
            XROrigin.transform.position = Vector3.Lerp(XROrigin.transform.position, target, Time.deltaTime);
        }
    }
    public void Pull()
    {
        target = XROrigin.transform.position + (this.transform.position - XROrigin.transform.position) * 0.75f;
        isPulling = true;
        TrackedPoseDriver driver = Camera.main.GetComponent<TrackedPoseDriver>();
        driver.trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
    }

    public void Release()
    {
        isPulling = false;
        TrackedPoseDriver driver = Camera.main.GetComponent<TrackedPoseDriver>();
        driver.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;
    }
}
