using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    [SerializeField] private GameObject[] portions;
    [SerializeField] int index = 0;
    public bool IsFinished => index == portions.Length;
    // Start is called before the first frame update
    void Start()
    {
        SetVisuals();
    }

    void SetVisuals()
    {
        for (int i = 0; i < portions.Length; i++)
        {
            portions[i].SetActive(i == index);
        }
    }

    [ContextMenu("Consume")]
    public void Consume()
    {
        if (!IsFinished)
        {
            index++;
            SetVisuals();
        }
    }


}
