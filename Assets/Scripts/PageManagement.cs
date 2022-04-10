using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.IO;
public class PageManagement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject pageObject;
    private List<Page> pageList = new List<Page>();
    private int currentPage = 0;
    [SerializeField] Button nextButton, submitButton;

    public string nameOfFood { get; set; }
    private string outputPath = @"C:\temp\SurveyResult.csv";
    [SerializeField] private Text pageText;
    private int resultID;
    private int numOfColumns = 0;

    [SerializeField] private Slider slider;
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField]  private GameObject sensorialGroup;
    [SerializeField]  private GameObject emotionGroup;
    private static String sensorialTransitPage = "Sensorial";
    private static string emotionTransitPage = "Emotion";
    private static string[] generalAttributes = { "Liking" };
    private static string[] sensorialAttributes = { "Sweetness", "Creamy", "Milky", "Sourness", "Vanilla" };
    private static string[] emotionAttributes = { "Active", "Adventurous", "Aggressive", "Bored", "Calm",
    "Disgusted", "Enthusiastic", "Good", "Good-natured", "Guilty", "Happy", "Interested",
    "Joyful","Loving","Mild","Nostalgic","Pleasant","Satisfied","Secure","Tame",
    "Understanding","Warm","Wild","Worried"};
    private static string finishTransitPage = "Finish";
    private static string[] sliderAttribute = { "Liking", "Milky" };
    private static string[] allAttributes = generalAttributes.Concat(new[] { sensorialTransitPage }).Concat(sensorialAttributes).Concat(new[] { emotionTransitPage }).Concat(emotionAttributes).Concat(new[] { finishTransitPage }).ToArray();

    private static string[] transitPages = new[] { sensorialTransitPage, emotionTransitPage, finishTransitPage };
    private List<string> checkedAttributes;


    void initializePages()
    {
        for (int i = 0; i < allAttributes.Length; i++)
        {
            pageList.Add(new Page(i, sliderAttribute.Contains<string>(allAttributes[i]) , allAttributes[i]));
        }
    }
    void initializeColumns()
    {
        File.WriteAllText(outputPath, "ID,Name");
        for (int i = 0; i < allAttributes.Length; i++)
        {
            if (!transitPages.Contains<string>(allAttributes[i]))
            {
                File.AppendAllText(outputPath, "," + allAttributes[i]);
            }
        }
        File.AppendAllText(outputPath, Environment.NewLine);
    }
    void initializeOutputFile()
    {

        if(File.Exists(outputPath) && !string.IsNullOrEmpty(File.ReadAllText(outputPath)))
        {
            string[] columns = File.ReadAllLines(outputPath)[0].Split(',');
            // minus 2 for id and name
            if(columns.Length - 2 != numOfColumns)
            {
                //Automatically create file if not exist
                File.WriteAllText(outputPath, string.Empty); 
                initializeColumns();
            }
        }
        else 
        {
            initializeColumns();
        }

    }
    private  static bool FileInUse(string path)
    {
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                return false;
            }
        }
        catch
        {
            return true;
        }
    }
    private void Reset()
    {
        currentPage = 0;
        checkedAttributes = new List<string> {sensorialTransitPage, emotionTransitPage, finishTransitPage};
        checkedAttributes.AddRange(generalAttributes);
    }
    void DisplayError(string error)
    {
        pageText.text = error;
        nextButton.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        sensorialGroup.gameObject.SetActive(false);
        emotionGroup.gameObject.SetActive(false);
        toggleGroup.gameObject.SetActive(false);
        slider.gameObject.SetActive(false);
    }
    void OnDisable()
    {
        Reset();
        if(FileInUse(outputPath))
        {
            // Debug.LogError("try closing the csv file first and try again!");
            DisplayError("try closing the csv file first and try again!");
        }
        else
        {
            ChangeVisual();
        }
    }
    void Start()
    {

        this.numOfColumns = allAttributes.Length - transitPages.Length;
        Reset();
        nextButton.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(false);
        if(FileInUse(outputPath))
        {
            // Debug.LogError("try closing the csv file first and try again!");
            DisplayError("try closing the csv file first and try again!");
            return;
        }

        resultID = File.ReadAllLines(outputPath).Length;
        initializePages();
        initializeOutputFile();
        ChangeVisual();
        
    }

    private void ChangeVisual()
    {
        sensorialGroup.gameObject.SetActive(false);
        emotionGroup.gameObject.SetActive(false);
        while (!checkedAttributes.Contains(allAttributes[currentPage]))
        {
            currentPage++; 
        }
        if(allAttributes[currentPage] == "Sensorial")
        {
            toggleGroup.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
            sensorialGroup.gameObject.SetActive(true);
            pageText.text = "Choose sensories you experienced";
            Toggle[] toggles = sensorialGroup.GetComponentsInChildren<Toggle>();
            foreach(Toggle toggle in toggles)
            {
                toggle.isOn = false;
            }
            return;
        }
        else if(allAttributes[currentPage] == "Emotion")
        {
            toggleGroup.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
            emotionGroup.gameObject.SetActive(true);
            pageText.text = "Choose emotions you experienced";
            Toggle[] toggles = emotionGroup.GetComponentsInChildren<Toggle>();
            foreach(Toggle toggle in toggles)
            {
                toggle.isOn = false;
            }
            return;
        }


        if(allAttributes[currentPage] == finishTransitPage)
        {
            toggleGroup.gameObject.SetActive(false);
            slider.gameObject.SetActive(false);
            pageText.text = "Thank you for your answers!";
            nextButton.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(true);
        }
        else
        {
            pageText.text = "Rate your " + pageList[currentPage].type;
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
            if (!transitPages.Contains<string>(pageList[i].type))
            {
                string text = pageList[i].score == 0 ? "NULL" : pageList[i].score.ToString();
                File.AppendAllText(outputPath,  "," + text);
            }

        }
        File.AppendAllText(outputPath, Environment.NewLine);
        resultID = File.ReadAllLines(outputPath).Length;
        gameObject.SetActive(false);

    }
    public void ChangePage()
    {
        //Extract data first
        int score = 0;
        if (!sensorialGroup.activeSelf && !emotionGroup.activeSelf)
        {
            if (pageList[currentPage].IsSlider)
            {
                score = (int)slider.value;
            }
            else
            {
                score = int.Parse(toggleGroup.ActiveToggles().FirstOrDefault().name.Split(' ')[1]);
            }
            pageList[currentPage].score = score;
        }
        else
        {
            if(sensorialGroup.activeSelf)
            {
                Toggle[] toggles = sensorialGroup.GetComponentsInChildren<Toggle>();
                foreach(Toggle toggle in toggles)
                {
                    if(toggle.isOn)
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
