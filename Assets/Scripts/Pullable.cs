using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pullable : MonoBehaviour
{
    private MovementManager movementManager;
    void Start()
    {
        movementManager = GameObject.FindGameObjectWithTag("VR Origin").GetComponent<MovementManager>();
    }
    public void Pull()
    {
        movementManager.Pull(this.gameObject);
    }
    public void Release()
    {
        movementManager.Release();
    }
}
