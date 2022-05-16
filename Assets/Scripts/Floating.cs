using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float rotation;
    [SerializeField] private bool IsRandom = true;

    void Start()
    {
        if (IsRandom)
        {
            offset = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            rotation = Random.Range(-2f, 2f) / 10f;
        }
        transform.GetComponent<Rigidbody>().velocity = offset;
    }

    void Update()
    {
        transform.Rotate(rotation, rotation, rotation);
    }

    public void SetOffset(float x, float y, float z)
    {
        this.IsRandom = false;
        offset = new Vector3(x, y, z);
        transform.GetComponent<Rigidbody>().velocity = offset;
        rotation = Random.Range(-2f, 2f) / 10f;
    }

}