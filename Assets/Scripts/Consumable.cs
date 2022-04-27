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
    // Start is called before the first frame update
    void Start()
    {
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

    [ContextMenu("Consume")]
    public void Consume()
    {
        if (!IsFinished)
        {
            index++;
            audioSrc.Play();

            SetVisuals();
            if (index == portions.Length)
            {
                surveyCanvas.gameObject.SetActive(false);
                surveyCanvas.GetComponentInChildren<Text>().text = name.Substring(0, name.Length - 6);
                Debug.Log("Survey for: " + name.Substring(0, name.Length - 6));
                surveyCanvas.GetComponent<PageManagement>().nameOfFood = name;
                surveyCanvas.gameObject.SetActive(true);
            }
        }
    }


}