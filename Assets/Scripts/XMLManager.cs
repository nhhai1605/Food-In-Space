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
    public List<XMLFood> foodList = new List<XMLFood>();
    private string questionPath, foodPath;
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
            List<string> foodNames = new List<string>();
            reader.ReadToFollowing("Food");
            do
            {
                reader.MoveToFirstAttribute();
                string foodName = reader.Value;             
                reader.MoveToNextAttribute();
                int foodId = int.Parse(reader.Value);
                id = foodId;
                reader.MoveToNextAttribute();
                int quantity = int.Parse(reader.Value);

                //Check if the mentioned food in xml exist or not
                GameObject obj = foodObjects.Where(obj => obj.name == foodName).SingleOrDefault();
                //if exist
                if(obj != null) 
                {
                    // obj.SetActive(active == "Yes");
                    foodNames.Add(foodName);
                    for(int j = 0; j < quantity; j++)
                    {
                        GameObject obj2 = Instantiate(obj, SpawningLocation.transform.position, Quaternion.identity);
                        obj2.name = foodName + "-" + foodId;
                        obj2.SetActive(true);
                    }
                }
                else //if not exist, delete the food in the xml
                {
                    reader.Close();
                    XDocument doc = XDocument.Load(foodPath);
                    doc.Element("Foods").Elements("Food").Where(x => (string)x.Attribute("name") == foodName).Remove();
                    doc.Save(foodPath);
                    reader = XmlReader.Create(foodPath);

                }

            } while (reader.ReadToFollowing("Food"));
            reader.Close();

            //Existing foods but not mentioned in the xml will be created with default quantity = 1
            for (int i = 0; i < foodFolder.transform.childCount; i++)
            {
                if(!foodNames.Contains(foodFolder.transform.GetChild(i).name))
                {
                    id++;
                    XDocument doc = XDocument.Load(foodPath);
                    XElement root = new XElement("Food");
                    root.Add(new XAttribute("name", foodFolder.transform.GetChild(i).name));
                    root.Add(new XAttribute("id", id));
                    root.Add(new XAttribute("quantity", "1"));                   
                    doc.Element("Foods").Add(root);
                    doc.Save(foodPath);

                    //Create obj in the scene
                    GameObject obj = Instantiate(foodFolder.transform.GetChild(i).gameObject, SpawningLocation.transform.position, Quaternion.identity);
                    obj.name = foodFolder.transform.GetChild(i).name + "-" + id;
                    obj.SetActive(true);
                    
                }
            }
        }
        catch
        {
            Debug.Log("food xml does not exist so it will generate and run the default version");
            XmlWriter writer = XmlWriter.Create(foodPath);
            writer.WriteWhitespace("\n");
            writer.WriteStartElement("Foods");
            writer.WriteWhitespace("\n");
            for (int i = 0; i < foodFolder.transform.childCount; i++)
            {
                writer.WriteStartElement("Food");
                writer.WriteAttributeString("name", foodFolder.transform.GetChild(i).name);
                writer.WriteAttributeString("id", id.ToString());
                writer.WriteAttributeString("quantity", "1");              
                writer.WriteEndElement();
                writer.WriteWhitespace("\n");

                //Create obj in the scene
                GameObject obj = Instantiate(foodFolder.transform.GetChild(i).gameObject, SpawningLocation.transform.position, Quaternion.identity);
                obj.name = foodFolder.transform.GetChild(i).name + "-" + id;
                obj.SetActive(true);
                id++;
            }
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
        }
    }
    private void ReadQuestionXML()
    {
        List<int> foodIdList = new List<int>();
        XmlReader foodReader = XmlReader.Create(foodPath);
        foodReader.ReadToFollowing("Food");
        do
        {
            foodReader.MoveToFirstAttribute();
            //string foodName = reader.Value;
            foodReader.MoveToNextAttribute();
            foodIdList.Add(int.Parse(foodReader.Value));

        } while (foodReader.ReadToFollowing("Food"));
        foodReader.Close();

        XmlReader reader; 
        try
        {      
            reader = XmlReader.Create(questionPath);
            reader.ReadToFollowing("Food");
            List<int> questionIdList = new List<int>();

            do
            {             
                reader.MoveToFirstAttribute();
                //string foodName = reader.Value;
                reader.MoveToNextAttribute();
                string foodIdString = reader.Value;
                int foodId = 0;
                bool isInt = int.TryParse(foodIdString, out foodId);
                if (foodIdList.Contains(foodId) && isInt)
                {
                    List<string> questionNameList = new List<string>();
                    foodList.Add(new XMLFood(foodId));
                    questionIdList.Add(foodId);
                    int currentQuestion = 0;
                    reader.ReadToFollowing("Question");
                    do
                    {
                        reader.MoveToFirstAttribute();
                        string questionName = reader.Value;
                        //print(questionName);
                        if(allAttributes.Contains(questionName))
                        {
                            questionNameList.Add(questionName);
                            reader.MoveToNextAttribute();
                            string questionContent = reader.Value;
                            reader.MoveToNextAttribute();
                            string questionType = reader.Value;
                            reader.MoveToNextAttribute();
                            string questionSlider = reader.Value;
                            reader.MoveToNextAttribute();
                            bool questionActive = reader.Value == "Yes";
                            foodList.Last().questionList.Add(new XMLQuestion(questionName, questionContent, questionType, questionSlider, questionActive));
                            currentQuestion++;
                        }
                        else
                        {
                            reader.Close();
                            XDocument doc = XDocument.Load(questionPath);
                            doc.Element("Foods").Elements("Food").Where(x => (string)x.Attribute("id") == foodId.ToString())
                                .Elements("Question").Where(x => (string)x.Attribute("name") == questionName).Remove();
                            doc.Save(questionPath);
                            reader = XmlReader.Create(questionPath);
                            for(int i = 0; i < currentQuestion; i++)
                            {
                                reader.ReadToFollowing("Question");
                            }
                        }
                        
                    } while (reader.ReadToNextSibling("Question"));

                    //if missing any attribute, it will create the default question for that attribute
                    foreach (string attr in allAttributes)
                    {
                        if (!questionNameList.Contains(attr))
                        {
                            reader.Close();

                            XDocument doc = XDocument.Load(questionPath);
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
                            XElement root = new XElement("Question", new XAttribute("name", attr), new XAttribute("content", content), new XAttribute("type", type), new XAttribute("slider", "No"), new XAttribute("active", "Yes"));
                            doc.Element("Foods").Elements("Food").Where(x => (string)x.Attribute("id") == foodId.ToString()).First().Add(root);
                            doc.Save(questionPath);

                            reader = XmlReader.Create(questionPath);
                        }
                    }
                }
                else
                {
                    //if the food id in the question xml is not exist in the food xml, we can delete it or just ignore it
                    //here is the code for deleting it, we can comment these codes if we want just to ignore it
                    reader.Close();

                    XDocument doc = XDocument.Load(questionPath);
                    doc.Element("Foods").Elements("Food").Where(x => (string)x.Attribute("id") == foodId.ToString()).Remove();
                    doc.Save(questionPath);

                    reader = XmlReader.Create(questionPath);
                }
                

            } while (reader.ReadToFollowing("Food"));
            reader.Close();
            //If there is a food mentioned in food xml but not in question xml, it will create the default one
            foreach(int id in foodIdList)
            {
                if(!questionIdList.Contains(id))
                {
                    XDocument doc = XDocument.Load(questionPath);
                    XElement root = new XElement("Food", new XAttribute("id", id.ToString()));
                    for(int i = 0; i < allAttributes.Length; i++)
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
                        root.Add(new XElement("Question", new XAttribute("name", allAttributes[i]), new XAttribute("content", content), new XAttribute("type", type), new XAttribute("slider", "No"), new XAttribute("active", "Yes")));
                    }
                    doc.Element("Foods").Add(root);
                    doc.Save(questionPath);
                }
            }
        }
        catch
        {
            Debug.Log("question xml does not exist so it will run the default version of questions");

            reader = XmlReader.Create(foodPath);
            reader.ReadToFollowing("Food");

            XmlWriter writer = XmlWriter.Create(questionPath);
            writer.WriteWhitespace("\n");
            writer.WriteStartElement("Foods");
            writer.WriteWhitespace("\n");

            do
            {
                writer.WriteWhitespace("  ");
                writer.WriteStartElement("Food");


                reader.MoveToFirstAttribute();
                //string foodName = reader.Value;
                reader.MoveToNextAttribute();
                string foodId = reader.Value;
                writer.WriteAttributeString("id", foodId);
                writer.WriteWhitespace("\n");
                //question
                for (int j = 0; j < allAttributes.Length; j++)
                {
                    writer.WriteWhitespace("    ");
                    writer.WriteStartElement("Question");

                    writer.WriteAttributeString("name", allAttributes[j]);
                    //default Content for question
                    string content = "Rate your " + allAttributes[j] + " using the scale below (1=not at all to 5=extremely)";
                    writer.WriteAttributeString("content", content);
                    if (generalAttributes.Contains(allAttributes[j]))
                    {
                        writer.WriteAttributeString("type", "General");
                    }
                    else if (sensorialAttributes.Contains(allAttributes[j]))
                    {
                        writer.WriteAttributeString("type", "Sensorial");

                    }
                    else if (emotionAttributes.Contains(allAttributes[j]))
                    {
                        writer.WriteAttributeString("type", "Emotion");
                    }
                    writer.WriteAttributeString("slider", "No");
                    writer.WriteAttributeString("active", "Yes");
                    writer.WriteEndElement();
                    writer.WriteWhitespace("\n");
                }

                writer.WriteWhitespace("  ");
                writer.WriteEndElement();
                writer.WriteWhitespace("\n");

            } while (reader.ReadToFollowing("Food"));
            reader.Close();
                    
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
            return;
        }
        
    }
    void Awake()
    {
        foodPath = Application.persistentDataPath + "/SceneFoods.xml";
        questionPath = Application.persistentDataPath + "/SurveyQuestions.xml";
        allAttributes = generalAttributes.Concat(sensorialAttributes).Concat(emotionAttributes).ToArray();
        ReadFoodXML();
        ReadQuestionXML();

        

        //PrintAll();
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
        public string Name { get; set; }
        public string Content { get; set; }
        public string Slider { get; set; }
        public bool Active { get; set; }
        public XMLQuestion(string questionName, string questionContent, string questionType, string questionSlider, bool Active)
        {
            this.Type = questionType;
            this.Name = questionName;
            this.Content = questionContent;
            this.Slider = questionSlider;
            this.Active = Active;
        }
    }
    public void PrintAll()
    {
        foreach(XMLFood food in foodList)
        {
            foreach(XMLQuestion question in food.questionList)
            {
                Debug.Log(food.Id + " - " + question.Name + " - " + question.Content + " - " + question.Slider);
            }
        }
    }
}
