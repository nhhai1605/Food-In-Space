using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class Consumable : MonoBehaviour
{
    [SerializeField] private GameObject[] portions;
    [SerializeField] private int index = 0;
    [SerializeField] private float timeToConsumeEachPortion = 2f;
    public bool IsFinished => index == portions.Length;
    [SerializeField] private Canvas surveyCanvas;
    private AudioSource audioSrc;
    private bool IsGrabbed;
    private bool IsEating = false;
    [SerializeField]  private float dt = 0f;
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

    void Update()
    {
        if(IsEating)
        {
            dt += Time.deltaTime;
            if(dt >= timeToConsumeEachPortion)
            {
                index++;
                audioSrc.Play();
                SetVisuals();
                if (index == portions.Length)
                {
                    //if object doesnt have anything left, will delete it after the audio source finish.
                    //If the object has something left, like the bone of the ham, keep it
                    if (this.GetComponent<Renderer>() == null)
                    {
                        Destroy(gameObject, audioSrc.clip.length);
                    }
                    Debug.Log("Survey for: " + name);
                    surveyCanvas.GetComponentInChildren<Text>().text = name.Split('-')[3];
                    surveyCanvas.GetComponent<PageManager>().foodName = name.Split('-')[3];
                    surveyCanvas.GetComponent<PageManager>().foodId = int.Parse(name.Split('-')[0]);

                    //Set the name first then deactive and active again to activate OnEnabled
                    surveyCanvas.gameObject.SetActive(false);
                    surveyCanvas.gameObject.SetActive(true);
                    IsEating = false;
                }
                dt = 0;
            }
        }
    }
    [ContextMenu("Consume")]
    public void Consume()
    {
        if (!IsFinished && IsGrabbed)
        {
            IsEating = true;
            //index++;
            //audioSrc.Play();
            //SetVisuals();
            //if (index == portions.Length)
            //{
            //    //if object doesnt have anything left, will delete it after the audio source finish.
            //    //If the object has something left, like the bone of the ham, keep it
            //    if(this.GetComponent<Renderer>() == null)
            //    {
            //        Destroy(gameObject, audioSrc.clip.length);
            //    }
            //    Debug.Log("Survey for: " + name);
            //    surveyCanvas.GetComponentInChildren<Text>().text = name.Split('-')[3];
            //    surveyCanvas.GetComponent<PageManager>().foodName = name.Split('-')[3];
            //    surveyCanvas.GetComponent<PageManager>().foodId = int.Parse(name.Split('-')[0]);

            //    //Set the name first then deactive and active again to activate OnEnabled
            //    surveyCanvas.gameObject.SetActive(false);
            //    surveyCanvas.gameObject.SetActive(true);
            //}
        }
    }
    public void StopConsume()
    {
        IsEating = false;
        dt = 0f;
    }
}