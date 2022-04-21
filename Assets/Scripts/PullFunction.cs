using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullFunction : MonoBehaviour
{
    [SerializeField] GameObject XROrigin;
    private Transform target;
    void Start()
    {
        target = XROrigin.transform;
    }

    void Update()
    {
        XROrigin.transform.position = Vector3.Lerp(XROrigin.transform.position, target.position, Time.deltaTime);
    }
    public void Pull()
    {
        print(this.name + " Pulling");
        target = this.transform;
        
    }
}
