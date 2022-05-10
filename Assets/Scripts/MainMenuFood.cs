using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFood : MonoBehaviour
{
    [SerializeField]
    private int buildIndexOfScene;

    public void GoToScene()
    {
        SceneManager.LoadScene(buildIndexOfScene);
    }
}
