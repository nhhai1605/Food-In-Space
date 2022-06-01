using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTask2 : MonoBehaviour
{
    public GameObject uiObject;
    public GameObject uiObject2;
    public GameObject uiObject3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Main Camera")
        {
            uiObject.SetActive(false);
            uiObject2.SetActive(true);
            uiObject3.SetActive(true);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Main Camera")
        {
            uiObject.SetActive(true);

        }

    }
    // Start is called before the first frame update
    void Start()
    {
        uiObject.SetActive(false);
        uiObject2.SetActive(false);
        uiObject3.SetActive(false);

    }
}
