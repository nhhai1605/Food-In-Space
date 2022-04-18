using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionManagement : MonoBehaviour
{
    [SerializeField] private Canvas surveyCanvas;
    private PageManagement pageManagement;
    private int currentPage;
    private Text currentText;
    string[] instructions =
    {
        "Firstly, please rate your liking experience using the below sliders",//Liking instruciton

        "Now, please select the sensorial feeling you had currently", //SensorialTransit instruction go here
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
        pageManagement = surveyCanvas.GetComponent<PageManagement>();
        currentText = GetComponentInChildren<Text>();
        currentText.gameObject.SetActive(false);
        currentText.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!surveyCanvas.isActiveAndEnabled)
        {
            return;
        }

        Vector3 targetPosition = Camera.main.transform.TransformPoint(new Vector3(0, 0, CameraDistance));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        var lookAtPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        transform.LookAt(lookAtPos, Camera.main.transform.up);
        currentPage = pageManagement.currentPage;
        currentText.gameObject.SetActive(true);
        currentText.text = instructions[currentPage];
        
    }
}
