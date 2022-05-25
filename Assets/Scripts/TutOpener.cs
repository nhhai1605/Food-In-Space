using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutOpener : MonoBehaviour
{
    public GameObject Panel;
    public GameObject button_L;
    public GameObject button_R;
    public GameObject button_G;
    public GameObject button_T;
    // Start is called before the first frame update
    public void OpenPanel()
    {
        if(Panel != null)
        {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
            button_L.SetActive(true);
            button_R.SetActive(true);
            button_G.SetActive(true);
        }

    }
    public void CloseTut()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(false);
            button_T.SetActive(true);
        }

    }
    public void Close_L()
    {
        this.button_L.SetActive(false);
    }
    public void Close_R()
    {
        this.button_R.SetActive(false);
    }
    public void Close_G()
    {
        this.button_G.SetActive(false);
    }

}
