using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoTo : MonoBehaviour {

    public string loadSceneAdditive;

    private void Start()
    {
        if (loadSceneAdditive != null && loadSceneAdditive != "")
        {
            LoadSceneAdditive(loadSceneAdditive);
        }
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void LoadSceneAdditive(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }
}
