using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyCamera : MonoBehaviour
{
    [SerializeField]
    private Camera copyCamera;

    private Camera thisCamera;

    void Start()
    {
        thisCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        thisCamera.transform.position = copyCamera.transform.position;
        thisCamera.transform.rotation = copyCamera.transform.rotation;
    }
}
