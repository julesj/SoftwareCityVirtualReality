using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class PositionOverHeadset : MonoBehaviour
{

    private Transform headsetPos;

    // Use this for initialization
    void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void Update()
    {
        if (headsetPos)
        {
            if (headsetPos.position.x != 0)
            {
                gameObject.transform.position = new Vector3(headsetPos.position.x, headsetPos.position.y + 2, headsetPos.position.z);
            }

        }
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        headsetPos = VRTK_DeviceFinder.HeadsetTransform();
    }
}