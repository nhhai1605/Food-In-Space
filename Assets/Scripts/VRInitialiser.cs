using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

// Initialises the VR Environment
public class VRInitialiser : MonoBehaviour
{
    [SerializeField]
    private GameObject mockOrigin;
    [SerializeField]
    private GameObject mockOculusOrigin;

    void Start()
    {
        // Debug Info
        if (!XRSettings.isDeviceActive)
        {
            Debug.Log("No Headset plugged");
        }
        else if (XRSettings.isDeviceActive && (XRSettings.loadedDeviceName == "Mock HMD" || XRSettings.loadedDeviceName == "MockHMD Display"))
        {
            Debug.Log("Using Mock HMD");
            Instantiate(mockOrigin);
        }
        else
        {
            Debug.Log("Using Headset: " + XRSettings.loadedDeviceName);
            Instantiate(mockOculusOrigin);
        }

    }
}
