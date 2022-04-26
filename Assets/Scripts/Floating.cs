using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float rotation;

    void Start()
    {
        offset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        rotation = Random.Range(-2f, 2f) / 50f;
        transform.GetComponent<Rigidbody>().velocity = offset;
    }

    void Update()
    {
        transform.Rotate(rotation, rotation, rotation);
    }

}