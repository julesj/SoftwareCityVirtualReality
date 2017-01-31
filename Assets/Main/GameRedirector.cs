using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameRedirector : MonoBehaviour {
    int scenes;

	void Start () {
        scenes = SceneManager.sceneCount;
        bool isBar = false;
        for (int i=0; i < scenes; i++) {
            Debug.Log(SceneManager.GetSceneAt(i).name);
            if (SceneManager.GetSceneAt(i).name == "MainSceneContrBar")
            {
                isBar = true;
            }
        }
        if (FindObjectOfType<LifeCycle>() == null && !isBar)
        {
            Application.LoadLevel("MainScene");
        }
    }
	
}
