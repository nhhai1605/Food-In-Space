using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraSwitchUI : MonoBehaviour
{
    [SerializeField]
    private Canvas uiCanvas;

    private CameraSwitch cameraSwitch;

    private GameObject hideableUi;

    void Start()
    {
        cameraSwitch = GetComponent<CameraSwitch>();
        hideableUi = uiCanvas.transform.GetChild(1).gameObject;
    }

    public void SetCamera()
    {
        uiCanvas.worldCamera = cameraSwitch.currentCamera;
    }

    // GUI Button
    public void onSpectatorButtonPress(int id)
    {
        cameraSwitch.setCamera(id);
    }

    public void onSpectatorButtonOpenSurveyButton()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }

    public void onSpectatorButtonOpenConfigurationButton()
    {
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }

    public void onSpectatorButtonRestartSceneButton()
    {
        SceneManager.LoadScene(0);
    }

    public void onSpectatorButtonHideButton()
    {
        hideableUi.SetActive(!hideableUi.activeSelf);
    }
}
