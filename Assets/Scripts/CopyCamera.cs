using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyCamera : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    //[SerializeField]
    //private Vector3 positionOffset;
    [SerializeField] private float distance = 0.2f;
    // Update is called once per frame
    void Update()
    {
        //transform.position = copyCamera.transform.position + positionOffset;
        //transform.rotation = copyCamera.transform.rotation;

        transform.LookAt(mainCamera.transform, mainCamera.transform.up);
        transform.position = mainCamera.transform.position - mainCamera.transform.forward * distance;

    }
}
