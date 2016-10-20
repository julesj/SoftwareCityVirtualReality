using UnityEngine;
using System.Collections;

public class ScaleRotateActionBinding : MonoBehaviour {


    void Awake()
    {
        LifeCycle lifeCycle = FindObjectOfType<LifeCycle>();
        if (lifeCycle != null)
        {
            lifeCycle.OnInitHandler += OnInit;
        }
        
    }

    void OnInit () {
        Grow cityManager = FindObjectOfType<Grow>();
        cityManager.AddScaleSliderControl(transform.Find("Slider/Grip Scale").GetComponent<SliderControl>());
        cityManager.AddRotationSliderControl(transform.Find("Slider/Grip Rotate").GetComponent<SliderControl>());
    }


}
