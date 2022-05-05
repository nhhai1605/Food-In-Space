using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private int lowPolySceneBuildID = 1;

    public void launchLowPolyScene()
    {
        SceneManager.LoadScene(lowPolySceneBuildID);
    }

}
