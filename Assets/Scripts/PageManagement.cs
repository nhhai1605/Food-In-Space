using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PageManagement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject[] pages;
    private int index = 0;
    [SerializeField] Button nextButton, submitButton;
    private Slider slider;
    private ToggleGroup toggleGroup;
    void Start()
    {
        nextButton.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(false);
        toggleGroup = pages[index].GetComponentInChildren<ToggleGroup>();
        slider = pages[index].GetComponentInChildren<Slider>();
        ChangeVisual();
    }
    private void ChangeVisual()
    {

        for (int i = 0; i < pages.Length; i++)
        {
            if(pages[i] != null)
            {
                pages[i].SetActive(i == index);
            }
        }
        if(index == pages.Length - 1)
        {
            nextButton.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(true);
        }
    }
    public void SubmitSurvey()
    {
        Debug.Log("Survey submitted!");
        gameObject.SetActive(false);
    }
    public void ChangePage()
    {
        if(toggleGroup != null)
        {
            Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
            int score = int.Parse(toggle.name.Split(' ')[1]);
            Debug.Log("Page: " + index + " - Score: "  + score);
        }
        else if (slider != null)
        {
            int score = (int) slider.value;
            Debug.Log("Page: " + index + " - Score: "  + slider.value);
        }   
        else
        {
            Debug.LogError("Cannot find Slider or Toggle Group!");
            return;
        }
        index++;
        toggleGroup = pages[index].GetComponentInChildren<ToggleGroup>();
        slider = pages[index].GetComponentInChildren<Slider>();
        ChangeVisual();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
