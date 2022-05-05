using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitch : MonoBehaviour
{
    private bool spectatorCameraOn = true;
    [SerializeField]
    private Camera spectatorCamera = null;

    public InputActionReference buttonReference;
    // Start is called before the first frame update
    private void Awake()
    {
        buttonReference.action.started += SwitchCamera;
    }

    public void Start()
    {
        // spectatorCamera = GameObject.FindGameObjectWithTag("SpectatorCamera").GetComponent<Camera>();
    }

    private void onDestroy()
    {
        buttonReference.action.started -= SwitchCamera;
    }

    private void SwitchCamera(InputAction.CallbackContext context)
    {
        if (spectatorCameraOn)
        {
            spectatorCamera.depth = -1;
        }
        else
        {
            spectatorCamera.depth = 1;
        }
        spectatorCameraOn = !spectatorCameraOn;
    }
}
