﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class MoveSoftwareCity : MonoBehaviour {

    private VRTK_ControllerEvents controllerEvents;
    private bool isGrapped;
    private GameObject SoftwareCity;
    private Vector3 oldPos;
    private Vector3 delta;

    public float scaleFactor = 1;

	// Use this for initialization
	void Start () {
        controllerEvents = gameObject.GetComponentInParent<VRTK_ControllerEvents>();
        if (controllerEvents)
        {
            controllerEvents.GripPressed += ControllerEvents_GripPressed;
            controllerEvents.GripReleased += ControllerEvents_GripReleased;
        }
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SoftwareCity = GameObject.Find("SoftwareCity");
    }

    private void ControllerEvents_GripReleased(object sender, ControllerInteractionEventArgs e)
    {
        isGrapped = false;
    }

    private void ControllerEvents_GripPressed(object sender, ControllerInteractionEventArgs e)
    {
        isGrapped = true;
        oldPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update () {
		if (isGrapped && SoftwareCity)
        {
            delta = gameObject.transform.position - oldPos;
            SoftwareCity.transform.position += new Vector3(delta.x*scaleFactor, 0.0f, delta.z*scaleFactor);
            oldPos = gameObject.transform.position;
        }
	}

    public void SetIsGrapped(bool grap)
    {
        isGrapped = grap;
        oldPos = gameObject.transform.position;
    }
}
