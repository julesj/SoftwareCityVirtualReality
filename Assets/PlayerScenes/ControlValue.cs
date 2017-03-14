using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;
using VRTK.UnityEventHelper;
using UnityEngine.SceneManagement;

public class ControlValue : MonoBehaviour {

    public Text go;

    private VRTK_Control_UnityEvents controlEvents;
    private FloatModel scaleModel;

    private void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Szene " + scene.name + " wurde geladen");
        if (scene.name == "ScaleRotateExampleScene")
        {
            foreach (FloatModel model in FindObjectsOfType<FloatModel>())
            {
                if (model.name.Equals("Scale"))
                {
                    scaleModel = model;
                }
            }

            controlEvents = GetComponent<VRTK_Control_UnityEvents>();
            if (controlEvents == null)
            {
                controlEvents = gameObject.AddComponent<VRTK_Control_UnityEvents>();
            }

            controlEvents.OnValueChanged.AddListener(HandleChange);
        }
    }

    private void HandleChange(object sender, Control3DEventArgs e)
    {
        go.text = e.value.ToString() + "(" + e.normalizedValue.ToString() + "%)";
        scaleModel.SetValue(e.value / 100);
    }
}
