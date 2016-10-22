using UnityEngine;
using System.Collections;

public class GameRedirector : MonoBehaviour {

	void Start () {
	    if (FindObjectOfType<LifeCycle>() == null)
        {
            Application.LoadLevel("MainScene");
        }
	}
	
}
