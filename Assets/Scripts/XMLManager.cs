using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Linq;
public class XMLManager : MonoBehaviour
{
    
    // Start is called before the first frame update
    private XmlReader reader;
    private string path;
    public List<XMLFood> foodList = new List<XMLFood>() ;
    void Awake()
    {
        path = Application.persistentDataPath + "/SurveyQuestions.xml";
        string foodName, questionType, questionName, questionContent, questionSlider;
        try
        {
            reader = XmlReader.Create(path);
        }
        catch
        {
            Debug.Log("xml does not exist so it will run the default version");
            return;
        }
        reader.ReadToFollowing("Food");
        do
        {
            reader.MoveToFirstAttribute();
            foodName = reader.Value;
            foodList.Add(new XMLFood(foodName));
            //print(foodName);
            reader.ReadToFollowing("Question");
            do
            {
                reader.MoveToFirstAttribute();
                questionType = reader.Value;
                reader.MoveToNextAttribute();
                questionName = reader.Value;
                reader.MoveToNextAttribute();
                questionContent = reader.Value;
                reader.MoveToNextAttribute();
                questionSlider = reader.Value;
                //print(foodName + " " + questionType + " " + questionName + " " + questionSlider);
                foodList.Last().questionList.Add(new XMLQuestion(questionType, questionName, questionContent, questionSlider));

            } while (reader.ReadToNextSibling("Question"));

        } while (reader.ReadToFollowing("Food"));

        //PrintAll();
    }

    public class XMLFood
    {
        public string Name { get; set; }
        public List<XMLQuestion> questionList = new List<XMLQuestion>();

        public XMLFood(string foodName)
        {
            this.Name = foodName;
        }
    }
    public class XMLQuestion
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Slider { get; set; }
        public XMLQuestion(string questionType, string questionName, string questionContent, string questionSlider)
        {
            this.Type = questionType;
            this.Name = questionName;
            this.Content = questionContent;
            this.Slider = questionSlider;
        }
    }
    public void PrintAll()
    {
        foreach(XMLFood food in foodList)
        {
            foreach(XMLQuestion question in food.questionList)
            {
                Debug.Log(food.Name + " - " + question.Name + " - " + question.Content + " - " + question.Slider);
            }
        }
    }
}
