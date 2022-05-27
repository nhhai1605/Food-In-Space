using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class TutorialConsumable : MonoBehaviour
{
    [SerializeField] private GameObject[] portions;
    [SerializeField] private int index = 0;

    [SerializeField] GameObject task2, task3;
    public bool IsFinished => index == portions.Length;
    [SerializeField] private Canvas surveyCanvas;
    [SerializeField] private float timeToConsumeEachPortion = 2f;

    private AudioSource audioSrc;
    private bool IsGrabbed;
    private bool IsEating = false;
    private float dt = 0f;
    // Start is called before the first frame update
    void Start()
    {
        task3.SetActive(false);
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
        if (IsEating)
        {
            dt += Time.deltaTime;
            if (dt >= timeToConsumeEachPortion)
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
                    surveyCanvas.GetComponentInChildren<Text>().text = name;
                    //surveyCanvas.GetComponent<TutorialPageManager>().pageText.text = name;
                    task3.SetActive(true);
                    task2.SetActive(false);
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
        }
    }
    public void StopConsume()
    {
        IsEating = false;
        dt = 0f;
    }

}