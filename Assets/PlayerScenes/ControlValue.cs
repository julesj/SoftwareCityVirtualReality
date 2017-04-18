using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;
using VRTK.UnityEventHelper;
using UnityEngine.SceneManagement;

public enum TypeOfInteract
{
    slider,
    up,
    down
};

public enum ScaleOrRotate
{
    rotate,
    scale
}

public class ControlValue : MonoBehaviour {

    public Text go;
    public TypeOfInteract parent;
    public ScaleOrRotate type;

    private VRTK_Control_UnityEvents controlEvents;
    private VRTK_Button_UnityEvents buttonEvents;
    private FloatModel scaleModel;
    private FloatModel rotateModel;
    private float actualScale;
    private float actualRotation;

    private void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;

        if (parent == TypeOfInteract.slider)
        {
            Slider slider = gameObject.GetComponent<Slider>();
            slider.onValueChanged.AddListener(HandleChange);
        } else if (parent == TypeOfInteract.up)
        {
            Button up = gameObject.GetComponent<Button>();
            up.onClick.AddListener(HandleUp);
        } else if (parent == TypeOfInteract.down)
        {
            Button down = gameObject.GetComponent<Button>();
            down.onClick.AddListener(HandleDown);
        }
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals(ClipboardBar.LoadableScenes.ScaleRotateExampleScene.ToString()))
        {
            foreach (FloatModel model in FindObjectsOfType<FloatModel>())
            {
                if (model.name.Equals("Scale"))
                {
                    scaleModel = model;
                    actualScale = scaleModel.GetValue(); // 0...1
                    WriteText(actualScale * 100);
                }
                if (model.name.Equals("Rotate"))
                {
                    rotateModel = model;
                    actualRotation = rotateModel.GetValue(); //0...1
                    WriteText(actualRotation * 100);
                }
            }
        }
    }

    public void HandleChange(float value)
    {
        ChangeValue(value); //0...100
    }

    public void HandleUp()
    {
        string number = go.text;
        float newValue = float.Parse(number) + 0.5f;
        ChangeValue(newValue);
    }

    public void HandleDown()
    {
        string number = go.text;
        float newValue = float.Parse(number) - 0.5f;
        ChangeValue(newValue);
    }

    private void ChangeValue(float value)
    {
        if (type == ScaleOrRotate.scale)
        {
            scale(value);
        }
        else if (type == ScaleOrRotate.rotate)
        {
            rotate(value);
        }
        WriteText(value);
    }

    private void scale(float value)
    {
        if (scaleModel)
        {
            scaleModel.SetValue(value / 100, true);
        }
    }

    private void rotate(float value)
    {//hier Änderungen
        if (rotateModel && scaleModel)
        {
            if (scaleModel.GetValue() > 0.8)
            {
                rotateModel.SetValue(value / 100, true);
            } else
            {
                rotateModel.SetValue(value / 100);
            }
        }
    }

    private void WriteText(float value)
    {
        if (value > 0 && value < 100)
        {
            go.text = value.ToString("0.0");
            //text 0...100
        }
    }
}
