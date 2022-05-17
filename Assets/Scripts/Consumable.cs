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
                
                surveyCanvas.GetComponentInChildren<Text>().text = name;
                Debug.Log("Survey for: " + name);
                //try
                //{
                //    surveyCanvas.GetComponent<OldPageManagement>().nameOfFood = name;
                //}
                //catch
                //{
                //    Debug.LogWarning("Using New PageManagement");
                //}
                //try
                //{
                //    surveyCanvas.GetComponent<PageManagement>().nameOfFood = name;
                //}
                //catch
                //{
                //    Debug.LogWarning("Using Old PageManagement");
                //}
                surveyCanvas.GetComponent<PageManager>().nameOfFood = name;

                //Set the name first then deactive and active again to activate OnEnabled
                surveyCanvas.gameObject.SetActive(false);
                surveyCanvas.gameObject.SetActive(true);
            }
        }
    }


}