using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameRedirector : MonoBehaviour {
    int scenes;
    string[] redirectBar = { "Opening Scene", "Controller_WelcomeScene", "Gesten_WelcomeScene", "Mixed_WelcomeScene" };
    ClipboardBar clipboard;

	void Start () {
        scenes = SceneManager.sceneCount;
        bool isBar = false;
        for (int i=0; i < scenes; i++) {
            if (redirectBar.Contains(SceneManager.GetSceneAt(i).name) && 
                FindObjectOfType<ClipboardBar>() == null)
            {
                Debug.Log("Lädt MainSceneBar");
                SceneManager.LoadScene("MainSceneBar", LoadSceneMode.Additive);
                isBar = true;
            } else if (FindObjectOfType<ClipboardBar>() != null)
            {
                isBar = true;
            }
        }
        if (FindObjectOfType<LifeCycle>() == null && !isBar)
        {
            Debug.Log("Lädt MainScene");
            Application.LoadLevel("MainScene");
        }
    }
	
}
