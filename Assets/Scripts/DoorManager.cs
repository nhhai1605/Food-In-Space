using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private bool DoorIsOpen = false;
    [SerializeField] private bool ButtonIsPressedFirst = true;
    [SerializeField] private float DoorMoveDistance = 2f;
    [SerializeField] private float ButtonMoveDistance = 0.03f;
    [SerializeField] private float DoorOpenSpeed = 0.02f;
    [SerializeField] private Color ButtonActiveColor, ButtonInactiveColor;
    public bool IsToggle = true;
    [SerializeField] private float WaitTimeIfNotToggleInSecond = 2f;

    private bool ButtonIsBeingPressed = false, buttonMoved = false;
    private Vector3 initialPos;
    [SerializeField] private GameObject[] buttons;
    private int notToggleState = 0;
    private float timePassed = 0;

    public bool IsDoorOpen()
    {
        return DoorIsOpen;
    }
    public void SetOpenTime(float time)
    {
        WaitTimeIfNotToggleInSecond = time;
    }
    void Awake()
    {
        initialPos = this.transform.position;
        if (ButtonIsPressedFirst)
        {
            foreach(GameObject button in buttons)
            {
                button.GetComponent<Renderer>().material.color = ButtonInactiveColor;
            }
        }
        else
        {
            foreach (GameObject button in buttons)
            {
                button.GetComponent<Renderer>().material.color = ButtonActiveColor;
            }
        }
    }

    void FixedUpdate()
    {

        if (Vector3.Distance(initialPos, this.transform.position) < DoorMoveDistance && ButtonIsBeingPressed)
        {
            if (DoorIsOpen)
            {
                this.transform.position += this.transform.up * DoorOpenSpeed;
            }
            else
            {
                this.transform.position -= this.transform.up * DoorOpenSpeed;
            }
        }
        else if (Vector3.Distance(initialPos, this.transform.position) >= DoorMoveDistance && ButtonIsBeingPressed)
        {
            if (IsToggle)
            {
                ButtonIsBeingPressed = false;
                foreach (GameObject button in buttons)
                {
                    button.GetComponent<MeshCollider>().enabled = true;
                }
            }
            else
            {
                timePassed += Time.deltaTime;

                if (timePassed > WaitTimeIfNotToggleInSecond)
                {
                    timePassed = 0f;
                    ControlDoor();
                }           
            }
        }

        if (!ButtonIsPressedFirst)
        {
            if (buttonMoved)
            {
                foreach (GameObject button in buttons)
                {
                    button.transform.position = button.transform.position - button.transform.up * ButtonMoveDistance;
                    button.GetComponent<Renderer>().material.color = ButtonActiveColor;

                }
                buttonMoved = false;

            }
        }
        else
        {
            if (buttonMoved)
            {
                foreach (GameObject button in buttons)
                {
                    button.transform.position = button.transform.position + button.transform.up * ButtonMoveDistance;
                    button.GetComponent<Renderer>().material.color = ButtonInactiveColor;
                }
                
                buttonMoved = false;
            }
        }
    }

    public void ControlDoor()
    {
        if (!IsToggle)
        {
            notToggleState++;
            if (notToggleState < 3)
            {
                initialPos = this.transform.position;
                ButtonIsBeingPressed = true;
                buttonMoved = true;
                DoorIsOpen = !DoorIsOpen;
                ButtonIsPressedFirst = !ButtonIsPressedFirst;
                foreach (GameObject button in buttons)
                {
                    button.GetComponent<MeshCollider>().enabled = false;
                    button.GetComponent<AudioSource>().Play();
                }
                this.GetComponent<AudioSource>().Play();
                if (notToggleState == 2)
                {
                    timePassed = WaitTimeIfNotToggleInSecond;
                }
            }
            else 
            {
                notToggleState = 0;
                ButtonIsBeingPressed = false;
                foreach (GameObject button in buttons)
                {
                    button.GetComponent<MeshCollider>().enabled = true;

                }
            }
            return;
        }
        initialPos = this.transform.position;
        ButtonIsBeingPressed = true;
        buttonMoved = true;
        DoorIsOpen = !DoorIsOpen;
        ButtonIsPressedFirst = !ButtonIsPressedFirst;
        this.GetComponent<AudioSource>().Play();
        foreach (GameObject button in buttons)
        {
            button.GetComponent<MeshCollider>().enabled = false;
            button.GetComponent<AudioSource>().Play();
        }
    }

    public void InvokeControlDoor(float time)
    {
        Invoke("ControlDoor", time);
    }

}