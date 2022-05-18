using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InstructionManager : MonoBehaviour
{
    [SerializeField] private Canvas surveyCanvas;
    private PageManager pageManager;
    private int currentPage;
    private Text currentText;
    private string sliderInstruction = "1=disliked extrememly to 9=liked extremely";
    private string checkBoxInstruction = "1=not at all to 5=extremely";
    //private bool SurveyIsOn = false;
    string[] instructions =
    {
        "Firstly, please rate your liking experience",//Liking instruciton

        "Now, please select the sensorial feeling you had currently experienced while consuming the product", //SensorialTransit instruction go here
        "You have select Sweetness, now please rate how sweetness are you feeling", 
        "You have select Creamy, now please rate how Creamy are you feeling", 
        "You have select Milky, now please rate how milky are you feeling",
        "You have select Sourness, now please rate how sourness are you feeling",
        "You have select Vanilla, now please rate how vanilla are you feeling",

        "Now, please select the emotion feeling you had during this food experience", //EmotionTransit instruction go here
        "You have select Active, now please rate how active are you feeling",
        "You have select Adventurous, now please rate how adventurous are you feeling",
        "You have select Aggressive, now please rate how aggressive are you feeling",
        "You have select Bored, now please rate how bored are you feeling",
        "You have select Calm, now please rate how calm are you feeling",
        "You have select Disgusted, now please rate how disgusted are you feeling",
        "You have select Enthusiastic, now please rate how enthusiastic are you feeling",
        "You have select Good, now please rate how good are you feeling",
        "You have select Good-natured, now please rate how good-natured are you feeling",
        "You have select Guilty, now please rate how guilty are you feeling",
        "You have select Happy, now please rate how happy are you feeling",
        "You have select Interested, now please rate how interested are you feeling",
        "You have select Joyful, now please rate how joyful are you feeling",
        "You have select Loving, now please rate how loving are you feeling",
        "You have select Mild, now please rate how mild are you feeling",
        "You have select Nostalgic, now please rate how nostalgic are you feeling",
        "You have select Pleasant, now please rate how pleasant are you feeling",
        "You have select Satisfied, now please rate how satisfied are you feeling",
        "You have select Secure, now please rate how secure are you feeling",
        "You have select Tame, now please rate how tame are you feeling",
        "You have select Understanding, now please rate how understanding are you feeling",
        "You have select Warm, now please rate how warm are you feeling",
        "You have select Wild, now please rate how wild are you feeling",
        "You have select Worried, now please rate how worried are you feeling",

        "Thank you for taking this survey" //finished the survey
    };
    private float CameraDistance = 0.2f;
    private float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        //pageManagement = surveyCanvas.GetComponent<OldPageManagement>();
        pageManager = surveyCanvas.GetComponent<PageManager>();
        currentText = GetComponentInChildren<Text>();
        currentText.gameObject.SetActive(true);
        currentText.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = Camera.main.transform.TransformPoint(new Vector3(0, 0, CameraDistance));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        var lookAtPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        transform.LookAt(lookAtPos, Camera.main.transform.up);
        if (!surveyCanvas.isActiveAndEnabled)
        {
            currentText.text = "Please take a couple sips of the sample and do the survey after consuming the product.";
        }
        else
        {
            currentPage = pageManager.currentPage;
            currentText.text = instructions[currentPage];
            if(!pageManager.transitPages.Contains(pageManager.pageList[currentPage].Key))
            {
                currentText.text += pageManager.pageList[currentPage].IsSlider ? " using the slider with the scale of " + sliderInstruction : " checking one box with the scale of " + checkBoxInstruction;
            }
        }
    }
}
