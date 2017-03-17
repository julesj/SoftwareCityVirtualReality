using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class PositionToHeadset : MonoBehaviour {

    private Transform headsetPos;
    private bool positioned = false;

	// Use this for initialization
	void Start () {
        SceneManager.sceneLoaded += SceneLoaded;
	}

    private void Update()
    {
        if (headsetPos)
        {
            if (headsetPos.position.x != 0 && !positioned)
            {
                gameObject.transform.position = new Vector3(headsetPos.position.x, headsetPos.position.y, headsetPos.position.z + 0.5f);
                positioned = true;
            }
            
        }
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        headsetPos = VRTK_DeviceFinder.HeadsetTransform();
    }
}
