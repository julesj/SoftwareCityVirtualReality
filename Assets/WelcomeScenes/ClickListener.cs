using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickListener : MonoBehaviour {
	
    public void GoTo(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
