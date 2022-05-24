using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    public GameObject _gameObject;
    bool active = true;

    public void Close()
    {
        if (active == true)
        {
            _gameObject.transform.gameObject.SetActive(false);
            active = false;
        }
    }
}
