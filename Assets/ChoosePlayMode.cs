using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChoosePlayMode : MonoBehaviour {
    Button gesten;
    Button contrJuRob;
    Button contrBar;
    Button mixedBar;
    GameObject panel;
    GameObject notImpl;
    ClipboardBar clipboard;

	// Use this for initialization
	void Start () {
        //Get all Objects
        panel = GameObject.Find("Panel");
        notImpl = GameObject.Find("NotImplemented");
        gesten = GameObject.Find("Gesten").GetComponent<Button>();
        contrBar = GameObject.Find("ControllerBar").GetComponent<Button>();
        contrJuRob = GameObject.Find("ControllerJuRob").GetComponent<Button>();
        mixedBar = GameObject.Find("MixedBar").GetComponent<Button>();

        //UI für Start
        notImpl.SetActive(false);

        //Button-Listener
        gesten.onClick.AddListener(ChooseGesture);
        contrBar.onClick.AddListener(ChooseContrBar);
        contrJuRob.onClick.AddListener(ChooseContrJuRob);
        mixedBar.onClick.AddListener(ChooseMixedBar);
    }

    private void Update()
    {
        if (FindObjectOfType<ClipboardBar>() != null && clipboard == null)
        {
            clipboard = GameObject.Find("Main").GetComponent<ClipboardBar>();
        }
    }

    void ChooseGesture()
    {
        notImpl.SetActive(true);
        panel.SetActive(false);
    }

    void ChooseContrBar()
    {
        Debug.Log("Lädt Controller_WelcomeScene");
        clipboard.setBefore("Opening Scene");
        SceneManager.LoadScene("Controller_WelcomeScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("Opening Scene");
    }

    void ChooseContrJuRob()
    {
        Debug.Log("Lädt Gesten_WelcomeScene");
        clipboard.setBefore("Opening Scene");
        SceneManager.LoadScene("MainScene");
    }

    void ChooseMixedBar()
    {
        Debug.Log("Lädt Mixed_WelcomeScene");
        clipboard.setBefore("Opening Scene");
        SceneManager.LoadScene("Mixed_WelcomeScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("Opening Scene");
    }
}
