using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class PositionToHeadset : MonoBehaviour {

    private Transform headsetPos;
    private bool positioned = false;
    private bool sceneIsLoaded = false;

    public float xDistanceFromHeadset = 0.0f;
    public float yDistanceFromHeadset = 0.0f;
    public float zDistanceFromHeadset = 0.0f;

    public bool followHeadset = true;
    public bool waitingForScene = true;
    public string waitForScene;

    private void Start()
    {
        if (waitingForScene)
        {
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name == waitForScene)
        {
            sceneIsLoaded = true;
        }
    }

    private void Update()
    {
        if (sceneIsLoaded && waitingForScene)
        {
            headsetPos = VRTK_DeviceFinder.HeadsetTransform();
        } else if (!waitingForScene)
        {
            headsetPos = VRTK_DeviceFinder.HeadsetTransform();
        }
        
        if (headsetPos)
        {
            if (headsetPos.position.x != 0 && !positioned)
            {
                Debug.Log("HeadsetPos: " + headsetPos.position);
                Debug.Log("Old Pos: " + gameObject.transform.position);
                gameObject.transform.position = new Vector3(headsetPos.position.x + xDistanceFromHeadset, headsetPos.position.y + yDistanceFromHeadset, headsetPos.position.z + zDistanceFromHeadset);
                Debug.Log("New Pos: " + gameObject.transform.position);
                if (!followHeadset)
                {
                    positioned = true;
                }
            }
        }
    }
}
