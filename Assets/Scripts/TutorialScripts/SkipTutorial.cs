using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipTutorial : MonoBehaviour
{
    [SerializeField] private GameObject taskActivator;
    [SerializeField] private GameObject[] VROriginList;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Skip()
    {
        taskActivator.SetActive(false);
        foreach(GameObject obj in VROriginList)
        {
            obj.transform.position = Vector3.zero;
        }

    }
}
