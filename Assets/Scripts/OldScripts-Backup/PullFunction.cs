using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
public class PullFunction : MonoBehaviour
    {
    [SerializeField] private GameObject VROrigin;
    private Camera VRCamera;

    [SerializeField] private float pullingSpeed = 1f;
    [SerializeField] private float pullingDistance = 20f;
    TrackedPoseDriver driver;
    private Vector3 dir = Vector3.zero;
    void Start()
    {
        VRCamera = Camera.main;
        driver = VRCamera.GetComponent<TrackedPoseDriver>();
    }

    void Update()
    {

        if(Vector3.Distance(VRCamera.transform.position, this.transform.position) > pullingDistance)
        {
            VROrigin.transform.position += dir * Time.deltaTime * pullingSpeed;
        }

    }
    public void Pull()
    {
        dir = this.transform.position - VRCamera.transform.position;
        driver.trackingType = TrackedPoseDriver.TrackingType.RotationOnly;
    }

    public void Release()
    {
        dir = Vector3.zero;
        driver.trackingType = TrackedPoseDriver.TrackingType.RotationAndPosition;
    }
}
