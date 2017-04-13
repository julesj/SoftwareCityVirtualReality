using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class PositionToHeadset : MonoBehaviour {

    private Transform headsetTransform;
    private bool positioned;
    private bool sceneIsLoaded;

    public float rightDistance = 0.0f;
    public float upDistance = 0.0f;
    public float forwardDistance = 0.0f;

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
            if (headsetTransform.position.x != 0 && !positioned)
            {
                if (gameObject.GetComponentInParent<Camera>())
                {//Relativ Verschiebung zum headset
                    gameObject.transform.localPosition = new Vector3(rightDistance, upDistance, forwardDistance);
                } else
                {//Weltkoordinaten
                    gameObject.transform.position = headsetTransform.position + headsetTransform.right * rightDistance + headsetTransform.up * upDistance + headsetTransform.forward * forwardDistance;
                }
                gameObject.transform.LookAt(headsetTransform);
            }
            if (!followHeadset)
            {
                positioned = true;
            }
            else
            {
                positioned = false;
            }
        }
    }

    public float[] GetFactors()
    {
        return new float[] { rightDistance, upDistance, forwardDistance };
    }
}
