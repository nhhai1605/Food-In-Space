using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.IO;
public class PageManager : MonoBehaviour
{
    [SerializeField] private XMLManager xmlManager;
    [SerializeField] private GameObject pageObject;
    public List<Page> pageList = new List<Page>();
    public int currentPage { get; set; } = 0;
    [SerializeField] Button nextButton, submitButton;

    public string nameOfFood { get; set; }
    private string outputPath;
    [SerializeField] private Text pageText;
    private int resultID;
    private int numOfColumns = 0;

    [SerializeField] private Slider slider;
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private GameObject sensorialGroup;
    [SerializeField] private GameObject emotionGroup;
    private string sliderInstruction = "(1=disliked extrememly to 9=liked extremely)";
    private string checkBoxInstruction = "(1=not at all to 5=extremely)";
    private string[] generalAttributes = { "Liking" };
    private string[] sensorialAttributes = { "Sweetness", "Creamy", "Milky", "Sourness", "Vanilla" };
    private string[] emotionAttributes = { "Active", "Adventurous", "Aggressive", "Bored", "Calm",
                                                    "Disgusted", "Enthusiastic", "Good", "Good-natured",
                                                    "Guilty", "Happy", "Interested","Joyful","Loving","Mild",
                                                    "Nostalgic","Pleasant","Satisfied","Secure","Tame",
                                                    "Understanding","Warm","Wild","Worried"};
    private string sensorialTransitPage = "Sensorial";
    private string emotionTransitPage = "Emotion";
    private string finishTransitPage = "Finish";
    private string[] sliderAttribute = {};
    private string[] allAttributes;
    public string[] transitPages { get; set; }
    private List<string> checkedAttributes;

    private List<XMLManager.XMLFood> foodList;

    List<string> xmlSensorialAttr, xmlEmotionAttr;
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
        pageList = new List<Page>();
        for (int i = 0; i < allAttributes.Length; i++)
        {
            pageList.Add(new Page(i, sliderAttribute.Contains(allAttributes[i]), allAttributes[i]));

        }
    }
    void InitializeColumns()
    {
        File.WriteAllText(outputPath, "ID,Name");
        for (int i = 0; i < allAttributes.Length; i++)
        {
            if (!transitPages.Contains(allAttributes[i]))
            {
                File.AppendAllText(outputPath, "," + allAttributes[i]);
            }
        }
        File.AppendAllText(outputPath, Environment.NewLine);
    }
    void InitializeOutputFile()
    {

        if (File.Exists(outputPath) && !string.IsNullOrEmpty(File.ReadAllText(outputPath)))
        {
            string[] columns = File.ReadAllLines(outputPath)[0].Split(',');
            // minus 2 for id and name
            if (columns.Length - 2 != numOfColumns)
            {
                //Automatically create file if not exist
                File.WriteAllText(outputPath, string.Empty);
                InitializeColumns();
            }
        }
        else
        {
            InitializeColumns();
        }

    }
    private static bool FileInUse(string path)
    {
        try
        {
            using FileStream fs = new FileStream(path, FileMode.OpenOrCreate);

            return false;
        }
        catch
        {
            return true;
        }
    }
    private void Reset()
    {
        currentPage = 0;
        checkedAttributes = new List<string> { sensorialTransitPage, emotionTransitPage, finishTransitPage };
        checkedAttributes.AddRange(generalAttributes);
        XMLManager.XMLFood food = null;
        foreach (XMLManager.XMLFood f in foodList)
        {
            if (f.Name == nameOfFood)
            {
                food = f;
                break;
            }
        }
        //Empty or we can make another default slider attribute variable
        this.sliderAttribute = Array.Empty<string>();
        if (food != null)
        {
            xmlSensorialAttr = new List<string>();
            xmlEmotionAttr = new List<string>();
            List<string> xmlSliderAttr = new List<string>();
            foreach (XMLManager.XMLQuestion question in food.questionList)
            {
                if (allAttributes.Contains(question.Name))
                {
                    if (question.Type == "Sensorial")
                    {
                        xmlSensorialAttr.Add(question.Name);
                    }
                    else if (question.Type == "Emotion")
                    {
                        xmlEmotionAttr.Add(question.Name);
                    }
                    else
                    {
                        Debug.LogError("Wrong question type!");
                    }
                    if (question.Slider == "Yes")
                    {
                        xmlSliderAttr.Add(question.Name);
                    }
                }
                else
                {
                    Debug.LogError("Name of question is out of scope");
                }
            }
            this.sliderAttribute = xmlSliderAttr.ToArray();
        }
        else
        {
            xmlSensorialAttr = sensorialAttributes.ToList();
            xmlEmotionAttr = emotionAttributes.ToList();
        }
        Toggle[] toggles1 = sensorialGroup.GetComponentsInChildren<Toggle>(true);
        foreach (Toggle toggle in toggles1)
        {
            if (xmlSensorialAttr.Contains(toggle.GetComponentInChildren<Text>().text))
            {
                toggle.gameObject.SetActive(true);
            }
            else
            {
                toggle.gameObject.SetActive(false);
            }
        }
        Toggle[] toggles2 = emotionGroup.GetComponentsInChildren<Toggle>(true);
        foreach (Toggle toggle in toggles2)
        {
            if (xmlEmotionAttr.Contains(toggle.GetComponentInChildren<Text>().text))
            {
                toggle.gameObject.SetActive(true);
            }
            else
            {
                toggle.gameObject.SetActive(false);
            }
        }
        InitializePages();
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

    void OnEnable()
    {
        Reset();
        if (FileInUse(outputPath))
        {
            DisplayError("Try closing the csv file first and try again!");
        }
        else
        {
            ChangeVisual();
        }
    }
    void Awake()
    {
        allAttributes = generalAttributes.Concat(new[] { sensorialTransitPage }).Concat(sensorialAttributes).Concat(new[] { emotionTransitPage }).Concat(emotionAttributes).Concat(new[] { finishTransitPage }).ToArray();
        transitPages = new[] { sensorialTransitPage, emotionTransitPage, finishTransitPage };
        foodList = xmlManager.foodList;
        outputPath = Application.persistentDataPath + "/SurveyResult.csv";
    }
    void Start()
    {        
        this.numOfColumns = allAttributes.Length - transitPages.Length;
        nextButton.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(false);
        if (FileInUse(outputPath))
        {
            DisplayError("Try closing the csv file first and try again!");
            return;
        }
        resultID = File.ReadAllLines(outputPath).Length;
        InitializeOutputFile();
        
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
            foreach (XMLManager.XMLFood f in foodList)
            {
                if (f.Name == nameOfFood)
                {
                    foreach(XMLManager.XMLQuestion q in f.questionList)
                    {
                        //print(q.Name + " - " + pageList[currentPage].Name);
                        if (q.Name == pageList[currentPage].Name)
                        {
                            if(q.Content != "")
                            {
                                pageText.text = q.Content;
                            }
                            break;
                        }
                        
                    }
                    break;
                }
            }
            toggleGroup.gameObject.SetActive(!pageList[currentPage].IsSlider);
            slider.gameObject.SetActive(pageList[currentPage].IsSlider);
            slider.value = 5;
            toggleGroup.GetComponentsInChildren<Toggle>()[0].isOn = true;
            nextButton.gameObject.SetActive(true);
            submitButton.gameObject.SetActive(false);
        }
    }
    public void SubmitSurvey()
    {
        ChangeVisual();
        // Debug.Log("Survey submitted!");
        File.AppendAllText(outputPath, resultID + "," + nameOfFood);
        for (int i = 0; i < pageList.Count; i++)
        {
            if (!transitPages.Contains(pageList[i].Name))
            {
                string text = pageList[i].Score == 0 ? "NULL" : pageList[i].Score.ToString();
                File.AppendAllText(outputPath, "," + text);
            }

        }
        File.AppendAllText(outputPath, Environment.NewLine);
        resultID = File.ReadAllLines(outputPath).Length;
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
