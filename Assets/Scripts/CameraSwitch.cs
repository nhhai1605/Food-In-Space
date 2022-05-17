using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitch : MonoBehaviour
{
    private bool spectatorCameraOn = true;
    private Camera currentCamera = null;
    [SerializeField]
    private Camera spectatorCamera = null;

    [SerializeField]
    private List<Camera> spectatorCameras = null;

    [SerializeField]
    private InputActionAsset inputActions;
    private InputActionMap cameraKeyboardInputActionMap;
    private string cameraKeyboardActionMapName = "Camera Keyboard";
    public InputActionReference buttonReference;
    // Start is called before the first frame update
    private void Awake()
    {

        setupCameraControls();
        spectatorCameras[0].enabled = true;
        spectatorCameras[0].depth = 1;

        buttonReference.action.started += SwitchCamera;
    }

    private void setupCameraControls()
    {
        // Get the Camera Keyboard Action Maps
        foreach (InputActionMap inputActionMap in inputActions.actionMaps)
        {
            if (inputActionMap.name.Equals(cameraKeyboardActionMapName))
            {
                cameraKeyboardInputActionMap = inputActionMap;
            }
        }

        if (cameraKeyboardInputActionMap == null)
        {
            Debug.LogError("Can't Find 'Camera Keyboard' Action Map!");
            return;
        }

        // Assign Button Events 
        cameraKeyboardInputActionMap.actions[0].performed += SwitchCamera; // VRCamera
        cameraKeyboardInputActionMap.actions[1].performed += onSpectatorCameraButtonPress; // Spectator Camera 1
        cameraKeyboardInputActionMap.actions[2].performed += onSpectatorCameraButtonPress; // Spectator Camera 2
        cameraKeyboardInputActionMap.actions[3].performed += onSpectatorCameraButtonPress; // etc...
        cameraKeyboardInputActionMap.actions[4].performed += onSpectatorCameraButtonPress;
        cameraKeyboardInputActionMap.actions[5].performed += onSpectatorCameraButtonPress;
        cameraKeyboardInputActionMap.actions[6].performed += onSpectatorCameraButtonPress;
        cameraKeyboardInputActionMap.actions[7].performed += onSpectatorCameraButtonPress;

    }

    private void onDestroy()
    {
        buttonReference.action.started -= SwitchCamera;
    }

    private void onSpectatorCameraButtonPress(InputAction.CallbackContext context)
    {
        // Confirm which button was pressed
        int cameraID = int.Parse(context.action.name[context.action.name.Length - 1].ToString());

        if (cameraID > spectatorCameras.Count)
            return;

        // Reset Camera
        if (currentCamera != null)
        {
            currentCamera.depth = -1;
            currentCamera.enabled = false;
        }

        // Set new Camera
        currentCamera = spectatorCameras[cameraID - 1];
        currentCamera.enabled = true;
        currentCamera.depth = 1;
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
