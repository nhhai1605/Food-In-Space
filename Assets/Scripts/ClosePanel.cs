using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    public GameObject gameObject;
    bool active = true;

    public void Close()
    {
        if (active == true)
        {
            gameObject.transform.gameObject.SetActive(false);
            active = false;
        }
    }
}
