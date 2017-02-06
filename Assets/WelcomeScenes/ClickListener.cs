using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickListener : MonoBehaviour {

    Button ok;

	// Use this for initialization
	void Start () {
        ok = GameObject.Find("OK_Button").GetComponent<Button>();
	}
	
    public void GoTo(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
