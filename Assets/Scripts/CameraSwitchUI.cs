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

    void Awake()
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
        // EditorUtility.RevealInFinder(Application.persistentDataPath);
        Application.OpenURL(Application.persistentDataPath);
    }

    public void onSpectatorButtonOpenConfigurationButton()
    {
        // EditorUtility.RevealInFinder(Application.persistentDataPath);
        Application.OpenURL(Application.persistentDataPath);
    }

    public void onSpectatorButtonReturnToMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void onSpectatorButtonRestartSceneButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void onSpectatorButtonHideButton()
    {
        hideableUi.SetActive(!hideableUi.activeSelf);
    }
}
