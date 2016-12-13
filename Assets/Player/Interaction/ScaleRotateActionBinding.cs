using UnityEngine;
using System.Collections;
using System;

public class ScaleRotateActionBinding : MonoBehaviour {


    void Awake()
    {
        EventBus.Register(this);
    }

    public void OnEvent(SceneReadyEvent e) {
        transform.Find("Slider/Grip Scale").GetComponent<SliderControl>().OnSliderMoveHanders += OnSliderMoved;
        transform.Find("Slider/Grip Rotate").GetComponent<SliderControl>().OnSliderMoveHanders += OnSliderMoved;

        foreach (FloatModel model in FindObjectsOfType<FloatModel>())
        {
            if (model.name.Equals("Scale"))
            {
                transform.Find("Slider/Grip Scale").GetComponent<SliderControl>().SetModel(model);
             } else if (model.name.Equals("Rotate"))
            {
                transform.Find("Slider/Grip Rotate").GetComponent<SliderControl>().SetModel(model);
            }
        }
    }

    private void OnSliderMoved(SliderControl sliderControl)
    {
        Hint.Confirm("HowToScaleTranslate1Hint");
        Hint.Confirm("HowToScaleTranslate2Hint");
        Hint.Confirm("ScaleTranslateTriggerHint");
    }
}
