using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{ 
    [SerializeField] private Vector3 offset;
    [SerializeField] private float rotation;
    
    void Start () 
    {
        offset =  new Vector3(Random.Range(-20f, 20f), Random.Range(50f , 100f),Random.Range(-20f, 20f));
        rotation = Random.Range(-4f, 4f) / 50f;
        transform.GetComponent<Rigidbody>().velocity = offset;
    }
     
    void Update () 
    {
        transform.Rotate(rotation,rotation,rotation);
    }

}