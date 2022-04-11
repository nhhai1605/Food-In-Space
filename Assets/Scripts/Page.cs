using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Page 
{
    public int pageNumber{ get; }
    public string type{ get; }
    public bool IsSlider{ get; }
    public int score = 0;
    public Page(int pageNumber, bool IsSlider, string type)
    {
        this.pageNumber = pageNumber;
        this.IsSlider = IsSlider;
        this.type = type;
    }

}