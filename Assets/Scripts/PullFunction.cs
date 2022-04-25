using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
public class PullFunction : MonoBehaviour
    {
    [SerializeField] GameObject XROrigin;
    private Transform target;
    public bool isPulling = false;
    void Start()
    {
        target = XROrigin.transform;
    }

    void Update()
    {
        if(isPulling)
            XROrigin.transform.position = Vector3.Lerp(XROrigin.transform.position, target.position, Time.deltaTime / 5f);
    }
    public void Pull()
    {
        target = this.transform;
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
