using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Slider mainSlider;
    [SerializeField] Text mainText;
    void Start()
    {
        mainSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    public void ValueChangeCheck()
    {
        mainText.text = "Current Value: " + mainSlider.value;
    }
}
