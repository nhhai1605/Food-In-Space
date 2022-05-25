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

    public string foodName { get; set; }
    public int foodId { get; set; }
    private string outputPath;
    [SerializeField] private Text pageText;
    private int resultID;
    private int numOfColumns = 0;

    [SerializeField] private Slider slider;
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private GameObject sensorialGroup;
    [SerializeField] private GameObject emotionGroup;

    private string sensorialTransitPage = "Sensorial";
    private string emotionTransitPage = "Emotion";
    private string finishTransitPage = "Finish";
    private string[] sliderAttribute = {};
    private string[] allAttributes;
    public string[] transitPages { get; set; }
    private List<string> checkedAttributes;

    private List<XMLManager.XMLFood> foodList;
    private XMLManager.XMLFood currFood;
    List<string> xmlSensorialAttr, xmlEmotionAttr, xmlGeneralAttr;
    public class Page
    {
        public int PageNumber { get; }
        public string Key { get; }
        public bool IsSlider { get; }
        public int Score = 0;
        public Page(int pageNumber, bool IsSlider, string key)
        {
            this.PageNumber = pageNumber;
            this.IsSlider = IsSlider;
            this.Key = key;
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
        currFood = null;
        foreach (XMLManager.XMLFood f in foodList)
        {
            if (f.Id == foodId)
            {
                currFood = f;
                break;
            }
        }

        xmlSensorialAttr = new List<string>();
        xmlEmotionAttr = new List<string>();
        xmlGeneralAttr = new List<string>();
        List<string> activeAttr = new List<string>();
        List<string> xmlSliderAttr = new List<string>();
        foreach (XMLManager.XMLQuestion question in currFood.questionList)
        {
            //print($"{question.Key}-{question.Slider}-{question.Active}-{question.Content}");
            if (question.Active)
            {
                activeAttr.Add(question.Key);
            }
            if (question.Type == "Sensorial")
            {
                xmlSensorialAttr.Add(question.Key);
            }
            else if (question.Type == "Emotion")
            {
                xmlEmotionAttr.Add(question.Key);
            }
            else if (question.Type == "General")
            {
                xmlGeneralAttr.Add(question.Key);
            }
            else
            {
                Debug.LogError("Wrong question type!");
            }
            if (question.Slider)
            {
                xmlSliderAttr.Add(question.Key);
            }

        }
        sliderAttribute = xmlSliderAttr.ToArray();

        Toggle[] toggles = sensorialGroup.GetComponentsInChildren<Toggle>(true).Concat(emotionGroup.GetComponentsInChildren<Toggle>(true)).ToArray();
        foreach (Toggle toggle in toggles)
        {
            toggle.isOn = false;
            if (activeAttr.Contains(toggle.GetComponentInChildren<Text>().text))
            {
                toggle.interactable = true;
                toggle.GetComponentInChildren<Image>().color = Color.white;
            }
            else
            {
                toggle.interactable = false;
                toggle.GetComponentInChildren<Image>().color = Color.gray;
            }
        }
       
        allAttributes = xmlGeneralAttr.Concat(new[] { sensorialTransitPage }).Concat(xmlSensorialAttr).Concat(new[] { emotionTransitPage }).Concat(xmlEmotionAttr).Concat(new[] { finishTransitPage }).ToArray();
        checkedAttributes = new List<string> { sensorialTransitPage, emotionTransitPage, finishTransitPage };
        //checkedAttributes.AddRange(xmlGeneralAttr);
        foreach(string general in xmlGeneralAttr)
        {
            if(activeAttr.Contains(general))
            {
                checkedAttributes.Add(general);
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
        transitPages = new[] { sensorialTransitPage, emotionTransitPage, finishTransitPage };
        foodList = xmlManager.foodList;
        outputPath = Application.persistentDataPath + "/SurveyResult-" + DateTime.Now.ToString("yyyy-MM-dd-HH_mm_ss") + ".csv";
        print(outputPath);
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
        InitializeOutputFile();
        resultID = File.ReadAllLines(outputPath).Length;


    }

    private void ChangeVisual()
    {
        sensorialGroup.SetActive(false);
        emotionGroup.SetActive(false);
        while (!checkedAttributes.Contains(allAttributes[currentPage]))
        {
            currentPage++;
        }
        if (allAttributes[currentPage] == sensorialTransitPage)
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
            nextButton.gameObject.SetActive(true);
            submitButton.gameObject.SetActive(false);
            return;
        }
        else if (allAttributes[currentPage] == emotionTransitPage)
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
            nextButton.gameObject.SetActive(true);
            submitButton.gameObject.SetActive(false);
            return;
        }
        else if (allAttributes[currentPage] == finishTransitPage)
        {
            toggleGroup.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
            pageText.text = "Thank you for your answers!";
            nextButton.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(true);
        }
        else
        {
            foreach (XMLManager.XMLQuestion q in currFood.questionList)
            {
                if (q.Key == pageList[currentPage].Key)
                {
                    pageText.text = q.Content;
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
        File.AppendAllText(outputPath, resultID + "," + foodName);
        for (int i = 0; i < pageList.Count; i++)
        {
            if (!transitPages.Contains(pageList[i].Key))
            {
                string text = pageList[i].Score == 0 ? "NULL" : pageList[i].Score.ToString();
                File.AppendAllText(outputPath, "," + text);
            }

        }
        File.AppendAllText(outputPath, Environment.NewLine);
        resultID = File.ReadAllLines(outputPath).Length;
        gameObject.SetActive(false);

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

}
