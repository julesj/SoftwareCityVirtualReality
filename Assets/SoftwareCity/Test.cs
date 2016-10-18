using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        FindObjectOfType<LifeCycle>().OnBeginHandler += DoOnBegin;
    }

    private void DoOnBegin()
    {
        Debug.Log("test");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
