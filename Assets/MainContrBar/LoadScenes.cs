using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneManager.LoadScene("SoftwareCityScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("EnvironmentScene", LoadSceneMode.Additive);
        SceneManager.LoadScene("PlayerSceneBar", LoadSceneMode.Additive);
    }
}
