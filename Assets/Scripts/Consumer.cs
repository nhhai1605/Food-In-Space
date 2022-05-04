using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumer : MonoBehaviour
{
    private Collider _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = false;
    }



    private void OnCollisionEnter(Collision other)
    {
        Consumable consumable = other.collider.GetComponent<Consumable>();
        if (consumable != null)
        {
            consumable.Consume();
        }
    }

}
