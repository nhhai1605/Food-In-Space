using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialConsumer : MonoBehaviour
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
        TutorialConsumable consumable = other.collider.GetComponent<TutorialConsumable>();
        if (consumable != null)
        {
            consumable.Consume();
        }

    }

    private void OnCollisionExit(Collision other)
    {
        TutorialConsumable consumable = other.collider.GetComponent<TutorialConsumable>();
        if (consumable != null)
        {
            consumable.StopConsume();
        }
    }
}