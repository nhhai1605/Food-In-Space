using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System;
using System.IO;
public class XMLManager : MonoBehaviour
{

    // Start is called before the first frame update
    public List<XMLFood> foodList = new List<XMLFood>();
    private string questionPath, foodPath, logPath;
    private int id = 1;
    [SerializeField] private GameObject foodFolder, SpawningLocation;

    private string[] generalAttributes = { "Liking" };
    private string[] sensorialAttributes = { "Sweetness", "Creamy", "Milky", "Sourness", "Vanilla" };
    private string[] emotionAttributes = { "Active", "Adventurous", "Aggressive", "Bored", "Calm",
                                                    "Disgusted", "Enthusiastic", "Good", "Good-natured",
                                                    "Guilty", "Happy", "Interested","Joyful","Loving","Mild",
                                                    "Nostalgic","Pleasant","Satisfied","Secure","Tame",
                                                    "Understanding","Warm","Wild","Worried"};
    private string[] allAttributes;

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

            XmlReader reader = XmlReader.Create(foodPath);
            reader.Close();
            List<string> foodNames = new List<string>();
            List<int> foodIds = new List<int>();
            XDocument doc = XDocument.Load(foodPath, LoadOptions.SetLineInfo);
            List<XElement> foodElements = doc.Element("Foods").Elements("Food").ToList();
            foreach(XElement foodElement in foodElements)
            {
                IXmlLineInfo info = foodElement;
                try
                {
                    string foodIdString = foodElement.Attribute("id").Value;
                    string mesh = foodElement.Attribute("mesh").Value;
                    string name = foodElement.Attribute("name").Value;
                    string quantityString = foodElement.Attribute("quantity").Value;
                    string orderString = foodElement.Attribute("order").Value;
                    if(foodIds.Contains(int.Parse(foodIdString))) // existed
                    {
                        LogOutput(logPath, $"[ERROR]: The Food at line {info.LineNumber}, with ID: {foodIdString}, already existed!");
                        foodElement.ReplaceWith(new XComment(foodElement.ToString()));
                    }
                    else // not existed
                    {
                        GameObject obj = foodObjects.Where(obj => obj.name == mesh).FirstOrDefault();
                        if (obj == null)
                        {
                            LogOutput(logPath, $"[ERROR]: The mesh in the Food XML at line {info.LineNumber} cannot be found!");
                            foodElement.ReplaceWith(new XComment(foodElement.ToString()));
                        }
                        else
                        {
                            foodNames.Add(mesh);
                            foodIds.Add(int.Parse(foodIdString));
                            id = int.Parse(foodIdString);
                        }
                    }
 
                }
                catch (Exception ex)
                {
                    LogOutput(logPath, $"[ERROR]: Invalid input or missing some parameters in Food XML at line {info.LineNumber}");
                    foodElement.ReplaceWith(new XComment(foodElement.ToString()));
                    Debug.LogWarning(ex);
                }
            }
            //Create available food in the scene with quantity = 0
            for (int i = 0; i < foodFolder.transform.childCount; i++)
            {
                if (!foodNames.Contains(foodFolder.transform.GetChild(i).name))
                {
                    id++;
                    XElement foodElement = new XElement("Food");
                    foodElement.Add(new XAttribute("id", id));
                    foodElement.Add(new XAttribute("mesh", foodFolder.transform.GetChild(i).name));
                    foodElement.Add(new XAttribute("name", foodFolder.transform.GetChild(i).name));
                    foodElement.Add(new XAttribute("quantity", 0));
                    foodElement.Add(new XAttribute("order", 1));
                    doc.Element("Foods").Add(foodElement);
                    IXmlLineInfo info = foodElement;
                    LogOutput(logPath, $"[INFO]: Create a Food for an available but unmentioned mesh '{foodFolder.transform.GetChild(i).name}' with ID: {id} in Food XML");
                }
            }
            doc.Save(foodPath);

        }
        catch (Exception ex)
        {
            //Debug.Log("food xml does not exist so it will generate and run the default version");
            Debug.LogWarning(ex);
            XmlWriter writer = XmlWriter.Create(foodPath);
            writer.WriteWhitespace("\n");
            writer.WriteStartElement("Foods");
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
            XDocument doc = XDocument.Load(foodPath, LoadOptions.SetLineInfo);
            for(int i = 0; i < foodFolder.transform.childCount; i++)
            {
                XElement foodElement = new XElement("Food");
                foodElement.Add(new XAttribute("id", id));
                foodElement.Add(new XAttribute("mesh", foodFolder.transform.GetChild(i).name));
                foodElement.Add(new XAttribute("name", foodFolder.transform.GetChild(i).name));
                foodElement.Add(new XAttribute("quantity", 1));
                foodElement.Add(new XAttribute("order", id));
                doc.Element("Foods").Add(foodElement);
                id++;
            }
            doc.Save(foodPath);         
        }
    }
    private void ReadQuestionXML()
    {
        List<int> foodIdList = new List<int>();

        XDocument foodDoc = XDocument.Load(foodPath, LoadOptions.SetLineInfo);
        foreach(XElement e in foodDoc.Element("Foods").Elements("Food").ToList())
        {
            foodIdList.Add(int.Parse(e.Attribute("id").Value));
        }

        XmlReader reader = null; 
        try
        {
            reader = XmlReader.Create(questionPath);
            reader.Close();
            XDocument doc = XDocument.Load(questionPath, LoadOptions.SetLineInfo);
            List<int> questionIdList = new List<int>();
            List<XElement> foodElements = doc.Element("Foods").Elements("Food").ToList();
            foreach (XElement foodElement in foodElements)
            {
                int questionId = int.MinValue;
                try
                {
                    questionId = int.Parse(foodElement.Attribute("id").Value);
                    if (questionIdList.Contains(questionId) || !foodIdList.Contains(questionId)) //if already exist or id is invalid
                    {
                        IXmlLineInfo info = foodElement;
                        this.LogOutput(logPath, $"[ERROR]: The food with ID: {questionId} in Question XML at line {info.LineNumber} is invalid, already existed in this XML or not mentioned in the Food XML");
                        foodElement.ReplaceWith(new XComment(foodElement.ToString()));

                        //foodElement.Remove();
                    }
                    else //if not exist, then go into each question of the food
                    {
                        //Add to the id list
                        questionIdList.Add(questionId);
                        this.foodList.Add(new XMLFood(questionId));
                        List<string> questionKeys = new List<string>();
                        List<XElement> questionElements = foodElement.Elements("Question").ToList();
                        foreach (XElement questionElement in questionElements)
                        {
                            try
                            {
                                string questionKey = questionElement.Attribute("key").Value;
                                string content = questionElement.Attribute("content").Value;
                                string type = questionElement.Attribute("type").Value;
                                bool isSlider = questionElement.Attribute("slider").Value == "No";
                                bool isActive = questionElement.Attribute("active").Value == "Yes";
                                if (allAttributes.Contains(questionKey) && !questionKeys.Contains(questionKey)) //If the key is valid, then continue
                                {
                                    questionKeys.Add(questionKey);                                  
                                    foodList.Last().questionList.Add(new XMLQuestion(questionKey, content, type, isSlider, isActive));
                                }
                                else //If the key is invalid, comment and output the error in the log file, and maybe even delete the codes
                                {
                                    //delete the invalid question
                                    //questionElement.Remove();

                                    //Output to log file
                                    IXmlLineInfo info = questionElement;
                                    this.LogOutput(logPath, $"[ERROR]: the question Key at line {info.LineNumber} is invalid, please use the correct Key or check the spelling");
                                    questionElement.ReplaceWith(new XComment(questionElement.ToString()));
                                }
                            }
                            catch //Missing paramenter
                            {
                                IXmlLineInfo info = questionElement;
                                LogOutput(logPath, $"[ERROR]: Invalid input or missing some parameters in Question XML at line {info.LineNumber}");
                                questionElement.ReplaceWith(new XComment(questionElement.ToString()));

                            }
                        }
                        foreach (string attr in allAttributes)
                        {
                            //If some attribute existed but not in the xml, create the default one
                            if (!questionKeys.Contains(attr))
                            {
                                string type = "";
                                if (generalAttributes.Contains(attr))
                                {
                                    type = "General";
                                }
                                else if (sensorialAttributes.Contains(attr))
                                {
                                    type = "Sensorial";

                                }
                                else if (emotionAttributes.Contains(attr))
                                {
                                    type = "Emotion";
                                }
                                string content = "Rate your " + attr + " using the scale below (1=not at all to 5=extremely)";
                                XElement questionElement = new XElement("Question");
                                questionElement.Add(new XAttribute("key", attr));
                                questionElement.Add(new XAttribute("content", content));
                                questionElement.Add(new XAttribute("type", type));
                                questionElement.Add(new XAttribute("slider", "No"));
                                questionElement.Add(new XAttribute("active", "Yes"));
                                foodElement.Add(questionElement);
                                foodList.Last().questionList.Add(new XMLQuestion(attr, content, type, false, true));
                                this.LogOutput(logPath, $"[INFO]: Key '{attr}' for Food Id: {questionId} is missing, a default version for this Key will be generated");

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    IXmlLineInfo info = foodElement;
                    LogOutput(logPath, $"[ERROR]: Invalid input or missing some parameters in Food XML at line {info.LineNumber}");
                    foodElement.ReplaceWith(new XComment(foodElement.ToString()));
                    Debug.LogWarning(ex);
                }
                
               
            }
            //Now if food id is mentioned in the food xml but not in the question xml, create a default version for it

            foreach (int idx in foodIdList)
            {
                if (!questionIdList.Contains(idx))
                {
                    this.LogOutput(logPath, $"[INFO]: Questions for ID '{idx}' is missing, a default version for this ID will be generated");
                    XElement foodElement = new XElement("Food");
                    foodElement.Add(new XAttribute("id", idx));
                    this.foodList.Add(new XMLFood(idx));
                    for (int i = 0; i < allAttributes.Length; i++)
                    {
                        string type = "";
                        if (generalAttributes.Contains(allAttributes[i]))
                        {
                            type = "General";
                        }
                        else if (sensorialAttributes.Contains(allAttributes[i]))
                        {
                            type = "Sensorial";

                        }
                        else if (emotionAttributes.Contains(allAttributes[i]))
                        {
                            type = "Emotion";
                        }
                        string content = "Rate your " + allAttributes[i] + " using the scale below (1=not at all to 5=extremely)";
                        XElement questionElement = new XElement("Question");
                        questionElement.Add(new XAttribute("key", allAttributes[i]));
                        questionElement.Add(new XAttribute("content", content));
                        questionElement.Add(new XAttribute("type", type));
                        questionElement.Add(new XAttribute("slider", "No"));
                        questionElement.Add(new XAttribute("active", "Yes"));
                        foodElement.Add(questionElement);
                        foodList.Last().questionList.Add(new XMLQuestion(allAttributes[i], content, type, false, true));

                    }
                    doc.Element("Foods").Add(foodElement);
                }
            }
            doc.Save(questionPath);
         
        }
        catch  (Exception ex)
        {
            //Debug.LogWarning("The question xml does not exist or root element is invalid");
            Debug.LogWarning(ex);
            XmlWriter writer = XmlWriter.Create(questionPath);
            writer.WriteWhitespace("\n");
            writer.WriteStartElement("Foods");
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
            XDocument doc = XDocument.Load(questionPath, LoadOptions.SetLineInfo);
            foreach (int idx in foodIdList)
            {
                XElement foodElement = new XElement("Food");
                foodElement.Add(new XAttribute("id", idx));
                foodList.Add(new XMLFood(idx));
                foreach(string attr in allAttributes)
                {
                    XElement questionElement = new XElement("Question");
                    questionElement.Add(new XAttribute("key", attr));
                    string content = $"Rate your {attr} using the scale below (1=not at all to 5=extremely)";
                    questionElement.Add(new XAttribute("content", content));
                    string type = "";
                    if (generalAttributes.Contains(attr))
                    {
                        type = "General";
                    }
                    else if (sensorialAttributes.Contains(attr))
                    {
                        type = "Sensorial";

                    }
                    else if (emotionAttributes.Contains(attr))
                    {
                        type = "Emotion";
                    }
                    questionElement.Add(new XAttribute("type", type));
                    questionElement.Add(new XAttribute("slider", "No"));
                    questionElement.Add(new XAttribute("active", "Yes"));
                    foodElement.Add(questionElement);
                    foodList.Last().questionList.Add(new XMLQuestion(attr, content,type,false,true));
                }
                doc.Element("Foods").Add(foodElement);

            }
            doc.Save(questionPath);
        }
        
    }
    void Awake()
    {
        foodPath = Application.persistentDataPath + "/SceneFoods.xml";
        questionPath = Application.persistentDataPath + "/SurveyQuestions.xml";
        logPath = Application.persistentDataPath + "/Log.txt";

        //Initialize Log file everytime the game run
        File.WriteAllText(logPath, "Log generated at " + DateTime.Now);
        File.AppendAllText(logPath, Environment.NewLine);

        allAttributes = generalAttributes.Concat(sensorialAttributes).Concat(emotionAttributes).ToArray();
        ReadFoodXML();
        ReadQuestionXML();

        

        //PrintAll();
    }
    private void LogOutput(string path, string content)      
    {
        File.AppendAllText(path, content);
        File.AppendAllText(path, Environment.NewLine);
    }
    public class XMLFood
    {
        public List<XMLQuestion> questionList = new List<XMLQuestion>();
        public int Id;
        public XMLFood(int id)
        {
            this.Id = id;
        }
    }
    public class XMLQuestion
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public string Content { get; set; }
        public bool Slider { get; set; }
        public bool Active { get; set; }
        public XMLQuestion(string Key, string Content, string Type, bool Slider, bool Active)
        {
            this.Type = Key;
            this.Key = Content;
            this.Content = Type;
            this.Slider = Slider;
            this.Active = Active;
        }
    }
    public void PrintAll()
    {
        foreach(XMLFood food in foodList)
        {
            foreach(XMLQuestion question in food.questionList)
            {
                Debug.Log(food.Id + " - " + question.Key + " - " + question.Content + " - " + question.Slider);
            }
        }
    }
}
