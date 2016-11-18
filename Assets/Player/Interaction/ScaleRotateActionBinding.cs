using UnityEngine;
using System.Collections;

public class ScaleRotateActionBinding : MonoBehaviour {


    void Awake()
    {
        EventBus.Register(this);
    }

    public void OnEvent(SceneReadyEvent e) {
        Grow cityManager = FindObjectOfType<Grow>();
        cityManager.AddScaleSliderControl(transform.Find("Slider/Grip Scale").GetComponent<SliderControl>());
        cityManager.AddRotationSliderControl(transform.Find("Slider/Grip Rotate").GetComponent<SliderControl>());
    }


}
