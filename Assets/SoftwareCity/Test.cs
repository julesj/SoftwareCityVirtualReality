using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        EventBus.Register(this);
    }

    void OnEvent(SceneReadyEvent e)
    {
        Debug.Log("test");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
