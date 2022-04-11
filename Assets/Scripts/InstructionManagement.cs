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
        "First, use the slider to rate your liking experience",
        "Now, choose what sensorial feeling you had"
    };
    private float CameraDistance = 0.1f;
    private float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        pageManagement = surveyCanvas.GetComponent<PageManagement>();
        currentText = GetComponentInChildren<Text>();
        currentText.gameObject.SetActive(false);
        //currentText.transform.rotation = Quaternion.Euler(0,180,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!surveyCanvas.isActiveAndEnabled)
        {
            return;
        }

        //Vector3 targetPosition = Camera.main.transform.TransformPoint(new Vector3(0, 0, CameraDistance));
        //transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        //var lookAtPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
        //transform.LookAt(lookAtPos, Camera.main.transform.up);
        currentPage = pageManagement.currentPage;
        currentText.gameObject.SetActive(true);
        currentText.text = instructions[currentPage];
        
    }
}
