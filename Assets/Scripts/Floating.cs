using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{ 
    [SerializeField] private Vector3 offset;
    [SerializeField] private float rotation,  bounciness = 0.1f;
    
    void Start () 
    {
        offset =  new Vector3(Random.Range(-2f, 2f), Random.Range(10f , 20f),Random.Range(-2f, 2f)) / 50f;
        rotation = Random.Range(-2f, 2f) / 50f;
        transform.GetComponent<Rigidbody>().velocity = offset;
    }
     
    void Update () 
    {
        transform.Rotate(rotation,rotation,rotation);
    }

}