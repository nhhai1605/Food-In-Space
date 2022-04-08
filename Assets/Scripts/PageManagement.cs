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
    private Slider slider;
    private ToggleGroup toggleGroup;
    public string nameOfFood;
    private string outputPath = @"C:\temp\SurveyResult.txt";
    private string inputPath = @"C:\temp\SurveyQuestions.txt";
    private List<string> invalidLine = new List<string> { "", " " };
    private Text currentText;
    private int resultID;
    private int numOfColumns = 0;

    bool readFromInputPath()
    {
        List <string>stringList = File.ReadAllText(inputPath).Split(new string[] {Environment.NewLine}, StringSplitOptions.None).Except(invalidLine).ToList();
        while(true)
        {
            try
            {
                pageList.Add(new Page(numOfColumns, stringList[numOfColumns].Split(',')[1] == "1", stringList[numOfColumns].Split(',')[0]));
            }
            catch
            {
                Debug.LogError("Invalid format in input file! - Must be '[Type],[IsSlider, yes is 1 and no is 0]'. For example, 'Sweetness Liking,1'");
                return false;
            }
            numOfColumns++;
            if(numOfColumns == stringList.Count)
            {
                break;
            }
        }
        return true;
    }
    bool initializeColumns()
    {
        List <string>stringList = File.ReadAllText(inputPath).Split(new string[] {Environment.NewLine}, StringSplitOptions.None).Except(invalidLine).ToList();
        if(!readFromInputPath())
        {
            return false;
        }   
        File.AppendAllText(outputPath, "ID,Name,");
        for (int i = 0; i < numOfColumns; i++)
        {
            if(i == numOfColumns - 1)
            {
                File.AppendAllText(outputPath, stringList[i].Split(',')[0] + Environment.NewLine);
            }
            else
            {
                File.AppendAllText(outputPath, stringList[i].Split(',')[0] + ",");
            }
        }
        
        if (!File.Exists(inputPath))
        {
            Debug.LogError("Cannot find the question file!");
            return false;
        }
        return true;
    }
    bool initializeOutputFile()
    {
        string readText = File.ReadAllText(inputPath);
        List <string>stringList = readText.Split(new string[] {Environment.NewLine}, StringSplitOptions.None).Except(invalidLine).ToList();
        if(File.Exists(outputPath) && !string.IsNullOrEmpty(File.ReadAllText(outputPath)))
        {
            string[] columns = File.ReadAllLines(outputPath)[0].Split(',');
            if(columns.Length - 2 != stringList.Count)
            {
                //Automatically create file if not exist
                File.WriteAllText(outputPath, string.Empty); 
                return initializeColumns();
            }
            else
            {
                return readFromInputPath();
            }
        }
        else 
        {
            return initializeColumns();
        }
        
    }
    void Start()
    {
        if(!initializeOutputFile())
        {
            return;
        }
        resultID = File.ReadAllLines(outputPath).Length;
        nextButton.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(false);
        toggleGroup = pageObject.GetComponentInChildren<ToggleGroup>();
        slider = pageObject.GetComponentInChildren<Slider>();
        foreach(Text textObj in pageObject.GetComponentsInChildren<Text>())
        {
            if(textObj.name == "Text")
            {
                currentText = textObj;
            }
        }
        ChangeVisual();
    }

    private void ChangeVisual()
    {
        currentText.text = "Rate your " + pageList[currentPage].type;
        toggleGroup.gameObject.SetActive(!pageList[currentPage].IsSlider);
        slider.gameObject.SetActive(pageList[currentPage].IsSlider);
        slider.value = 5;
        toggleGroup.GetComponentsInChildren<Toggle>()[0].isOn = true;
        if(currentPage == pageList.Count - 1)
        {
            nextButton.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(true);
        }
        else
        {
            nextButton.gameObject.SetActive(true);
            submitButton.gameObject.SetActive(false);
        }
    }
    public void SubmitSurvey()
    {
        ChangePage();
        Debug.Log("Survey submitted!");
        File.AppendAllText(outputPath, resultID + "," + nameOfFood);
        for (int i = 0; i < pageList.Count; i++)
        {
            File.AppendAllText(outputPath,  "," + pageList[i].score.ToString());
        }
        File.AppendAllText(outputPath, Environment.NewLine);
        resultID = File.ReadAllLines(outputPath).Length;
        gameObject.SetActive(false);

    }
    public void ChangePage()
    {
        int score = 0;
        if(currentPage == 0)
        {
            
        }
        if(pageList[currentPage].IsSlider)
        {
            score = (int) slider.value;
        }
        else
        {
            score = int.Parse(toggleGroup.ActiveToggles().FirstOrDefault().name.Split(' ')[1]);
        }
        pageList[currentPage].score = score;
        currentPage++;
        if(currentPage == pageList.Count)
        {
            currentPage = 0;
        }
        ChangeVisual();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
