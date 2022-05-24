using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject VROrigin;
    [SerializeField] private List<GameObject> walls;
    [SerializeField] private MovementManager movementManager;

    void Start()
    {
        VROrigin = GameObject.FindGameObjectWithTag("VR Origin");
        movementManager = VROrigin.GetComponent<MovementManager>();
        
    }


    void OnCollisionStay(Collision collision)
    {
        //if (collision.collider.name == "main")
        if(walls.Contains(collision.collider.gameObject))
        {
            if (VROrigin.name == "MockOrigin")
            {
                Camera.main.GetComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>().trackingType = UnityEngine.InputSystem.XR.TrackedPoseDriver.TrackingType.RotationOnly;
            }
            else if (VROrigin.name == "OculusOrigin")
            {
                Camera.main.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>().trackingType = UnityEngine.SpatialTracking.TrackedPoseDriver.TrackingType.RotationOnly;
            }
            //Debug.DrawLine(this.transform.position, collision.GetContact(0).point);
            movementManager.ResetVelocity();
            Vector3 dir = transform.position - collision.GetContact(0).point;
            VROrigin.transform.position += dir;
           
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (walls.Contains(collision.collider.gameObject))
        {
            if (VROrigin.name == "MockOrigin")
            {
                Camera.main.GetComponent<UnityEngine.InputSystem.XR.TrackedPoseDriver>().trackingType = UnityEngine.InputSystem.XR.TrackedPoseDriver.TrackingType.RotationAndPosition;
            }
            else if (VROrigin.name == "OculusOrigin")
            {
                Camera.main.GetComponent<UnityEngine.SpatialTracking.TrackedPoseDriver>().trackingType = UnityEngine.SpatialTracking.TrackedPoseDriver.TrackingType.RotationAndPosition;
            }
        }
    }
}
