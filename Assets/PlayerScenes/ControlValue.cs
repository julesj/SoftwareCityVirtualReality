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
            controlEvents = GetComponent<VRTK_Control_UnityEvents>();
            if (controlEvents == null)
            {
                controlEvents = gameObject.AddComponent<VRTK_Control_UnityEvents>();
            }
            controlEvents.OnValueChanged.AddListener(HandleChange);
        } else if (parent == TypeOfInteract.up)
        {
            buttonEvents = GetComponent<VRTK_Button_UnityEvents>();
            if (buttonEvents == null)
            {
                buttonEvents = gameObject.AddComponent<VRTK_Button_UnityEvents>();
            }
            buttonEvents.OnPushed.AddListener(HandleUp);
        } else if (parent == TypeOfInteract.down)
        {
            buttonEvents = GetComponent<VRTK_Button_UnityEvents>();
            if (buttonEvents == null)
            {
                buttonEvents = gameObject.AddComponent<VRTK_Button_UnityEvents>();
            }
            buttonEvents.OnPushed.AddListener(HandleDown);
        }
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ScaleRotateExampleScene")
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

    private void HandleChange(object sender, Control3DEventArgs e)
    {
        ChangeValue(e.value);
    }

    private void HandleUp(object sender, Control3DEventArgs e)
    {
        string[] number = go.text.Split('(');
        float newValue = float.Parse(number[0]) + 0.5f;
        ChangeValue(newValue);
    }

    private void HandleDown(object sender, Control3DEventArgs e)
    {
        string[] number = go.text.Split('(');
        float newValue = float.Parse(number[0]) - 0.5f;
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
            scaleModel.SetValue(value / 100);
        }
    }

    private void rotate(float value)
    {
        if (rotateModel)
        {
            rotateModel.SetValue(value / 100);
        }
    }

    private void WriteText(float value)
    {
        go.text = value.ToString("0.0") + "(" + value.ToString("0.0") + "%)";
    }
}
