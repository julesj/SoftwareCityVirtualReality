using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateCanvas : MonoBehaviour {
    GameObject panel;

    void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        panel = GameObject.Find("UI_Panel");
        panel.SetActive(false);
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ScaleRotateExampleScene")
        {
            panel.SetActive(true);
        }
    }
}
