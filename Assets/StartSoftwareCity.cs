using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class StartSoftwareCity : MonoBehaviour {

    private string[] welcomeScenes = { "Controller_WelcomeScene", "Gesten_WelcomeScene", "Mixed_WelcomeScene" };

    public void StartCity()
    {
        bool isAlreadyLoaded = false;
        int count = SceneManager.sceneCount;
        for (int i = 0; i < count; i++)
        {
            if (welcomeScenes.Contains(SceneManager.GetSceneAt(i).name))
            {
                Debug.Log("Unload Scene: " + SceneManager.GetSceneAt(i).name);
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(i));
            }
            if (SceneManager.GetSceneAt(i).name.Equals(ClipboardBar.LoadableScenes.ScaleRotateExampleScene.ToString()))
            {
                isAlreadyLoaded = true;
            }
        }
        if (!isAlreadyLoaded)
        {
            SceneManager.LoadSceneAsync("ScaleRotateExampleScene", LoadSceneMode.Additive);
        }
    }
}
