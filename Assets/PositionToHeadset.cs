using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class PositionToHeadset : MonoBehaviour {

    private Transform headsetTransform;
    private bool positioned;
    private bool sceneIsLoaded;

    public float xDistanceFromHeadset = 0.0f;
    public float yDistanceFromHeadset = 0.0f;
    public float zDistanceFromHeadset = 0.0f;

    public bool followHeadset;
    public bool waitingForScene;
    public string waitForScene;

    private void Start()
    {
        positioned = false;
        sceneIsLoaded = false;
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
            headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
        } else if (!waitingForScene)
        {
            headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
        }

        
        
        if (headsetTransform)
        {
            Debug.Log("positioned: " + positioned);
            Debug.Log("Old Pos: " + gameObject.transform.position);
            if (headsetTransform.position.x != 0 && !positioned)
            {
                Debug.Log("HeadsetPos: " + headsetTransform.position);

                if (gameObject.GetComponentInParent<Camera>())
                {//Relativ Verschiebung zum headset
                    gameObject.transform.localPosition = new Vector3(xDistanceFromHeadset, yDistanceFromHeadset, zDistanceFromHeadset);
                } else
                {//Weltkoordinaten -> headsetTransform ist nicht in Weltkoordinaten!!
                    gameObject.transform.position = new Vector3(headsetTransform.position.x + xDistanceFromHeadset, headsetTransform.position.y + yDistanceFromHeadset, headsetTransform.position.z + zDistanceFromHeadset);
                    //gameobject.transform.rotation = headsetTransform.rotation;
                }
                Debug.Log("New Pos: " + gameObject.transform.position);
                Debug.Log("followHeadset: " + followHeadset);
                if (!followHeadset)
                {
                    positioned = true;
                }
            }
        }
    }
}
