using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraSwitch : MonoBehaviour
{

    public Camera currentCamera { private set; get; }
    private Camera vrSpectatorCamera = null;
    private CameraSwitchUI cameraSwitchUI = null;

    [SerializeField]
    private List<Camera> spectatorCameras = null;

    [SerializeField]
    private InputActionAsset inputActions;
    private InputActionMap cameraKeyboardInputActionMap;
    private string cameraKeyboardActionMapName = "Camera Keyboard";
    public InputActionReference buttonReference;
    // Start is called before the first frame update
    private void Start()
    {
        setupCameraControls();

        vrSpectatorCamera = GameObject.FindGameObjectWithTag("VRSpectatorCamera").GetComponent<Camera>();
        cameraSwitchUI = GetComponent<CameraSwitchUI>();

        setCamera(0);
    }

    public void setCamera(int id)
    {
        // 0 = VR
        // 1-9 = spectator
        if (id > spectatorCameras.Count)
            return;

        // Reset Camera
        if (currentCamera != null)
        {
            currentCamera.depth = -1;
            currentCamera.enabled = false;
        }

        // Vr Camera
        if (id == 0)
            currentCamera = vrSpectatorCamera;
        else
            currentCamera = spectatorCameras[id - 1];

        // Set new Camera
        currentCamera.enabled = true;
        currentCamera.depth = 1;
        cameraSwitchUI.SetCamera();
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
        cameraKeyboardInputActionMap.actions[0].performed += onSpectatorCameraButtonPress; // VRCamera
        cameraKeyboardInputActionMap.actions[1].performed += onSpectatorCameraButtonPress; // Spectator Camera 1
        cameraKeyboardInputActionMap.actions[2].performed += onSpectatorCameraButtonPress; // Spectator Camera 2
        cameraKeyboardInputActionMap.actions[3].performed += onSpectatorCameraButtonPress; // etc...
        cameraKeyboardInputActionMap.actions[4].performed += onSpectatorCameraButtonPress;
        cameraKeyboardInputActionMap.actions[5].performed += onSpectatorCameraButtonPress;
        cameraKeyboardInputActionMap.actions[6].performed += onSpectatorCameraButtonPress;
        cameraKeyboardInputActionMap.actions[7].performed += onSpectatorCameraButtonPress;
    }

    // Keyboard Button
    private void onSpectatorCameraButtonPress(InputAction.CallbackContext context)
    {
        // Confirm which button was pressed
        int cameraID = int.Parse(context.action.name[context.action.name.Length - 1].ToString());
        setCamera(cameraID);
    }
}
