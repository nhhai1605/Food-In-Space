using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pullable : MonoBehaviour
{
    private PullingManagement pullingManagement;
    void Start()
    {
        pullingManagement = GameObject.FindGameObjectWithTag("VR Origin").GetComponent<PullingManagement>();
    }
    public void Pull()
    {
        pullingManagement.Pull(this.gameObject);
    }
    public void Release()
    {
        pullingManagement.Release();
    }
}
