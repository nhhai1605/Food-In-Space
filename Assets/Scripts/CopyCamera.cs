using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyCamera : MonoBehaviour
{
    [SerializeField]
    private Camera copyCamera;

    [SerializeField]
    private Vector3 positionOffset;

    // Update is called once per frame
    void Update()
    {
        transform.position = copyCamera.transform.position + positionOffset;
        transform.rotation = copyCamera.transform.rotation;
    }
}
