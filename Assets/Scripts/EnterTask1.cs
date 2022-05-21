using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTask1 : MonoBehaviour
{
    public GameObject uiObject;
    public GameObject uiObject2;
    public GameObject uiObject3;
    public GameObject uiObject4;
    private void OnTriggerEnter()
    {
        uiObject.SetActive(true);
        uiObject2.SetActive(true);
    }
    private void OnTriggerExit()
    {
        uiObject.SetActive(false);
        uiObject2.SetActive(false);
        uiObject3.SetActive(true);
        uiObject4.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        uiObject.SetActive(false);
        uiObject2.SetActive(false);
        uiObject3.SetActive(false);
        uiObject4.SetActive(false);
    }

}
