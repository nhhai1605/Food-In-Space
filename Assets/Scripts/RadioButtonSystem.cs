using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class RadioButtonSystem : MonoBehaviour
{
    private ToggleGroup toggleGroup;
    // Start is called before the first frame update
    void Start()
    {
        toggleGroup = GetComponent<ToggleGroup>();
    }

    public void Submit()
    {
        Toggle toggle = toggleGroup.ActiveToggles().FirstOrDefault();
        Debug.Log(toggle.name + " - "  + toggle.GetComponentInChildren<Text>().text);
        Canvas[] canvasList = GameObject.FindObjectsOfType<Canvas>(true);
        foreach (Canvas c in canvasList)
        {
            if (c.name == "Food Rating Canvas")
            {
                c.gameObject.SetActive(false);
            }
        } 
    }
}
