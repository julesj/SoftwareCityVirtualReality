using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChoosePlayMode : MonoBehaviour {
    Button gesten;
    Button contrJuRob;
    Button contrBar;
    GameObject panel;
    GameObject notImpl;

	// Use this for initialization
	void Start () {
        //Get all Objects
        panel = GameObject.Find("Panel");
        notImpl = GameObject.Find("NotImplemented");
        gesten = GameObject.Find("Gesten").GetComponent<Button>();
        contrBar = GameObject.Find("ControllerBar").GetComponent<Button>();
        contrJuRob = GameObject.Find("ControllerJuRob").GetComponent<Button>();

        //UI für Start
        notImpl.SetActive(false);

        //Button-Listener
        gesten.onClick.AddListener(ChooseGesture);
        contrBar.onClick.AddListener(ChooseContrBar);
        contrJuRob.onClick.AddListener(ChooseContrJuRob);

    }

    void ChooseGesture()
    {
        notImpl.SetActive(true);
    }

    void ChooseContrBar()
    {
        SceneManager.LoadScene("MainSceneContrBar");
    }

    void ChooseContrJuRob()
    {
        SceneManager.LoadScene("MainScene");
    }
}
