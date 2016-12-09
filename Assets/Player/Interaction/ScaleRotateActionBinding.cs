using UnityEngine;
using System.Collections;

public class ScaleRotateActionBinding : MonoBehaviour {


    void Awake()
    {
        EventBus.Register(this);
    }

    public void OnEvent(SceneReadyEvent e) {
        foreach(FloatModel model in FindObjectsOfType<FloatModel>())
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


}
