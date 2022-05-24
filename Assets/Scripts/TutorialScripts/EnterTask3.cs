using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTask3 : MonoBehaviour
{
    public GameObject uiObject;
    public GameObject uiObject2;
    public GameObject uiObject3;
    public GameObject uiObject4;
    public GameObject taskActivator;

    private void OnTriggerEnter(Collider other)
    {
        uiObject.SetActive(false);
        uiObject2.SetActive(true);
        uiObject3.SetActive(true);
        uiObject4.SetActive(false);

    }
    private void OnTriggerExit( )
    {
        uiObject.SetActive(false);
        uiObject2.SetActive(false);
        uiObject3.SetActive(false);
        uiObject4.SetActive(false);
        taskActivator.SetActive(false);

    }
    void Start()
    {
        uiObject.SetActive(false);
        uiObject2.SetActive(false);
        uiObject3.SetActive(false);
        uiObject4.SetActive(false);

    }
    public void ActiveSuccess()
    {
        uiObject.SetActive(false);
        uiObject2.SetActive(false);
        uiObject3.SetActive(false);
        uiObject4.SetActive(true);
    }
}
