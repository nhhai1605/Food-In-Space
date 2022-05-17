using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private int lowPolySceneBuildID = 1;
    
    [SerializeField]
    private int highDefSceneBuildID = 2;

    public void launchLowPolyScene()
    {
        SceneManager.LoadScene(lowPolySceneBuildID);
    }

    public void launchHighDefScene() {
        SceneManager.LoadScene(highDefSceneBuildID);
    }

}
