using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Slider slider;
    [SerializeField] Text valueText;
    void Start()
    {
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        valueText.text = "Current Value: " + slider.value;
    }

    public void ValueChangeCheck()
    {
        valueText.text = "Current Value: " + slider.value;
    }
}
