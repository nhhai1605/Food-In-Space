using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System;

public class XMLManager : MonoBehaviour
{
    
    // Start is called before the first frame update
    public List<XMLFood> foodList = new List<XMLFood>() ;
    [SerializeField] private GameObject foodFolder;
    private void ReadFoodXML()
    {
        List<GameObject> foodObjects =  new List<GameObject>();
        for (int i = 0; i < foodFolder.transform.childCount; i++)
        {
            foodObjects.Add(foodFolder.transform.GetChild(i).gameObject);
        }
        try
        {
            //If the food mentioned in the xml is not exist in the scene, nothing will happen
            //But if the food exist in the scene but is not mentioned in the xml, the xml will update, and default will be active
            XmlReader reader = XmlReader.Create(Application.persistentDataPath + "/SceneFoods.xml");
            List<string> foodNames = new List<string>();
            reader.ReadToFollowing("Food");
            do
            {
                reader.MoveToFirstAttribute();
                string foodName = reader.Value;             
                reader.MoveToNextAttribute();
                string active = reader.Value;
                GameObject obj = foodObjects.Where(obj => obj.name == foodName).SingleOrDefault();
                if(obj != null) 
                {
                    obj.SetActive(active == "Yes");
                    foodNames.Add(foodName);
                }
                else
                {
                    reader.Close();
                    XDocument doc = XDocument.Load(Application.persistentDataPath + "/SceneFoods.xml");
                    doc.Element("Foods").Elements("Food").Where(x => (string)x.Attribute("name") == foodName).Remove();
                    doc.Save(Application.persistentDataPath + "/SceneFoods.xml");
                    reader = XmlReader.Create(Application.persistentDataPath + "/SceneFoods.xml");

                }

            } while (reader.ReadToFollowing("Food"));
            reader.Close();
            for (int i = 0; i < foodFolder.transform.childCount; i++)
            {
                if(!foodNames.Contains(foodFolder.transform.GetChild(i).name))
                {
                    XDocument doc = XDocument.Load(Application.persistentDataPath + "/SceneFoods.xml");
                    XElement root = new XElement("Food");
                    root.Add(new XAttribute("name", foodFolder.transform.GetChild(i).name));
                    root.Add(new XAttribute("active", "Yes"));                   
                    doc.Element("Foods").Add(root);
                    doc.Save(Application.persistentDataPath + "/SceneFoods.xml");
                    foodFolder.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        catch
        {
            Debug.Log("food xml does not exist so it will generate and run the default version");
            XmlWriter writer = XmlWriter.Create(Application.persistentDataPath + "/SceneFoods.xml");
            writer.WriteWhitespace("\n");
            writer.WriteStartElement("Foods");
            writer.WriteWhitespace("\n");
            for (int i = 0; i < foodFolder.transform.childCount; i++)
            {
                writer.WriteStartElement("Food");
                writer.WriteAttributeString("name", foodFolder.transform.GetChild(i).name);
                foodFolder.transform.GetChild(i).gameObject.SetActive(true);              
                writer.WriteAttributeString("active", "Yes");            
                writer.WriteEndElement();
                writer.WriteWhitespace("\n");
            }
            writer.WriteEndElement();
            writer.Flush();

        }
    }
    private void ReadQuestionXML()
    {
        XmlReader reader; 
        string foodName, questionType, questionName, questionContent, questionSlider;
        try
        {
            reader = XmlReader.Create(Application.persistentDataPath + "/SurveyQuestions.xml");
        }
        catch
        {
            Debug.Log("question xml does not exist so it will run the default version of questions");
            return;
        }
        reader.ReadToFollowing("Food");
        do
        {
            reader.MoveToFirstAttribute();
            foodName = reader.Value;
            foodList.Add(new XMLFood(foodName));
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
                foodList.Last().questionList.Add(new XMLQuestion(questionType, questionName, questionContent, questionSlider));

            } while (reader.ReadToNextSibling("Question"));

        } while (reader.ReadToFollowing("Food"));
        reader.Close();
    }
    void Awake()
    {
        ReadFoodXML();
        ReadQuestionXML();

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
