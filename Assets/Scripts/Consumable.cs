using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class Consumable : MonoBehaviour
{
    [SerializeField] private GameObject[] portions;
    [SerializeField] private int index = 0;
    public bool IsFinished => index == portions.Length;
    private Canvas canvas = null;
    private AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        SetVisuals();
        Canvas[] canvasList = GameObject.FindObjectsOfType<Canvas>(true);
        foreach (Canvas c in canvasList)
        {
            if (c.name == "Food Rating Canvas")
            {
                canvas = c;
            }
        } 
        if(canvas == null)
        {
            Debug.LogError("Cannot find Food Rating Canvas");
        }
    }

    void SetVisuals()
    {
        for (int i = 0; i < portions.Length; i++)
        {
            if(portions[i] != null)
            {
                portions[i].SetActive(i == index);
            }
        }
    }

    [ContextMenu("Consume")]
    public void Consume()
    {
        if (!IsFinished)
        {
            index++;
            audioSrc.Play();

            SetVisuals();
            if(index == portions.Length)
            {
                canvas.gameObject.SetActive(false);
                canvas.GetComponentInChildren<Text>().text = "Survey: " + name.Substring(0, name.Length - 6);
                canvas.GetComponent<PageManagement>().nameOfFood = name.Substring(0, name.Length - 6);
                canvas.gameObject.SetActive(true);    
            }
        }
    }


}
