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
    [SerializeField] private Canvas surveyCanvas;
    private AudioSource audioSrc;
    private bool IsGrabbed;
    // Start is called before the first frame update
    void Start()
    {
        IsGrabbed = false;
        audioSrc = GetComponent<AudioSource>();
        SetVisuals();
    }

    void SetVisuals()
    {
        for (int i = 0; i < portions.Length; i++)
        {
            if (portions[i] != null)
            {
                portions[i].SetActive(i == index);
            }
        }
    }
    public void Grab()
    {
        IsGrabbed = true;
    }
    public void Drop()
    {
        IsGrabbed = false; 
    }
    [ContextMenu("Consume")]
    public void Consume()
    {
        if (!IsFinished && IsGrabbed)
        {
            index++;
            audioSrc.Play();

            SetVisuals();
            if (index == portions.Length)
            {
                
                
                Debug.Log("Survey for: " + name);
                surveyCanvas.GetComponentInChildren<Text>().text = name.Split('-')[3];
                surveyCanvas.GetComponent<PageManager>().foodName = name.Split('-')[3];
                surveyCanvas.GetComponent<PageManager>().foodId = int.Parse(name.Split('-')[0]);

                //Set the name first then deactive and active again to activate OnEnabled
                surveyCanvas.gameObject.SetActive(false);
                surveyCanvas.gameObject.SetActive(true);
            }
        }
    }


}