using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.IO;
public class TutorialPageManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject pageObject;
    public List<Page> pageList = new List<Page>();
    public int currentPage { get; set; } = 0;
    [SerializeField] Button nextButton, submitButton;

    public string nameOfFood { get; set; }
    [SerializeField] private Text pageText;


    [SerializeField] private Slider slider;
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private GameObject sensorialGroup;
    [SerializeField] private GameObject emotionGroup;
    private string sensorialTransitPage = "Sensorial";
    private string emotionTransitPage = "Emotion";
    private string[] generalAttributes = { "Liking" };
    private string[] sensorialAttributes = { "Sweetness", "Creamy", "Milky", "Sourness", "Vanilla" };
    private string[] emotionAttributes = {   "Active", "Adventurous", "Aggressive", "Bored", "Calm",
                                                    "Disgusted", "Enthusiastic", "Good", "Good-natured",
                                                    "Guilty", "Happy", "Interested","Joyful","Loving","Mild",
                                                    "Nostalgic","Pleasant","Satisfied","Secure","Tame",
                                                    "Understanding","Warm","Wild","Worried"};
    private string finishTransitPage = "Finish";
    private string[] sliderAttribute = { "Liking", "Milky" };
    private string[] allAttributes;
    private string sliderInstruction = "(1=disliked extrememly to 9=liked extremely)";
    private string checkBoxInstruction = "(1=not at all to 5=extremely)";

    public string[] transitPages { get; set; }
    private List<string> checkedAttributes;

    public class Page
    {
        public int PageNumber { get; }
        public string Name { get; }
        public bool IsSlider { get; }
        public int Score = 0;
        public Page(int pageNumber, bool IsSlider, string name)
        {
            this.PageNumber = pageNumber;
            this.IsSlider = IsSlider;
            this.Name = name;
        }
    }

    void InitializePages()
    {
        for (int i = 0; i < allAttributes.Length; i++)
        {
            pageList.Add(new Page(i, sliderAttribute.Contains(allAttributes[i]), allAttributes[i]));
        }
    }

    private void Reset()
    {
        currentPage = 0;
        checkedAttributes = new List<string> { sensorialTransitPage, emotionTransitPage, finishTransitPage };
        checkedAttributes.AddRange(generalAttributes);
    }
    void DisplayError(string error)
    {
        pageText.text = error;
        GetComponentInChildren<Text>().text = "Error";
        nextButton.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        sensorialGroup.SetActive(false);
        emotionGroup.SetActive(false);
        toggleGroup.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        Reset();
        ChangeVisual();
    }
    void Awake()
    {
        allAttributes = generalAttributes.Concat(new[] { sensorialTransitPage }).Concat(sensorialAttributes).Concat(new[] { emotionTransitPage }).Concat(emotionAttributes).Concat(new[] { finishTransitPage }).ToArray();
        transitPages = new[] { sensorialTransitPage, emotionTransitPage, finishTransitPage };
    }
    void Start()
    {

        Reset();
        nextButton.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(false);

        InitializePages();
        ChangeVisual();

    }

    private void ChangeVisual()
    {
        sensorialGroup.SetActive(false);
        emotionGroup.SetActive(false);
        while (!checkedAttributes.Contains(allAttributes[currentPage]))
        {
            currentPage++;
        }
        if (allAttributes[currentPage] == "Sensorial")
        {
            toggleGroup.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
            sensorialGroup.SetActive(true);
            pageText.text = "Choose which sensorial attributes you experienced while consuming the product";
            Toggle[] toggles = sensorialGroup.GetComponentsInChildren<Toggle>();
            foreach (Toggle toggle in toggles)
            {
                toggle.isOn = false;
            }
            return;
        }
        else if (allAttributes[currentPage] == "Emotion")
        {
            toggleGroup.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
            emotionGroup.SetActive(true);
            pageText.text = "Choose which emotions you experienced while consuming the product";
            Toggle[] toggles = emotionGroup.GetComponentsInChildren<Toggle>();
            foreach (Toggle toggle in toggles)
            {
                toggle.isOn = false;
            }
            return;
        }


        if (allAttributes[currentPage] == finishTransitPage)
        {
            toggleGroup.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
            pageText.text = "Thank you for your answers!";
            nextButton.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(true);
        }
        else
        {
            pageText.text = "Rate your " + pageList[currentPage].Name + " using the scale below ";
            pageText.text += pageList[currentPage].IsSlider ? sliderInstruction : checkBoxInstruction;
            toggleGroup.gameObject.SetActive(!pageList[currentPage].IsSlider);
            slider.gameObject.SetActive(pageList[currentPage].IsSlider);
            slider.value = 5;
            toggleGroup.GetComponentsInChildren<Toggle>()[0].isOn = true;
            nextButton.gameObject.SetActive(true);
            submitButton.gameObject.SetActive(false);
        }
    }
    public void SubmitSurveyWithoutData()
    {
        ChangeVisual();
        gameObject.SetActive(false);
    }
    public void ChangePage()
    {
        //Extract data first
        if (!sensorialGroup.activeSelf && !emotionGroup.activeSelf)
        {
            int score;
            if (pageList[currentPage].IsSlider)
            {
                score = (int)slider.value;
            }
            else
            {
                score = int.Parse(toggleGroup.ActiveToggles().FirstOrDefault().name.Split(' ')[1]);
            }
            pageList[currentPage].Score = score;
        }
        else
        {
            if (sensorialGroup.activeSelf)
            {
                Toggle[] toggles = sensorialGroup.GetComponentsInChildren<Toggle>();
                foreach (Toggle toggle in toggles)
                {
                    if (toggle.isOn)
                    {
                        checkedAttributes.Add(toggle.GetComponentInChildren<Text>().text);
                    }
                }

            }
            else if (emotionGroup.activeSelf)
            {
                Toggle[] toggles = emotionGroup.GetComponentsInChildren<Toggle>();
                foreach (Toggle toggle in toggles)
                {
                    if (toggle.isOn)
                    {
                        checkedAttributes.Add(toggle.GetComponentInChildren<Text>().text);
                    }
                }
            }
        }
        //Then change to next page
        currentPage++;
        ChangeVisual();

    }
    // Update is called once per frame
    void Update()
    {

    }
}
