using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using VRTK;

public class MiniMapHandler : MonoBehaviour {

    Vector3 triggerPos;
    Vector3 oldPos;
    bool isTriggered;
    bool isGrabbed;
    VRTK_ControllerEvents controllerEvents;
    GameObject City;

    void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    // Update is called once per frame
    void Update () {
        if (isTriggered && isGrabbed)
        {
            Debug.Log("triggered + grabbed -> oldPos: " + oldPos + ", newPos: " + triggerPos);
            Vector3 shift = triggerPos - oldPos; //Global? -> y & z; für City z & x
            City.transform.position += new Vector3(shift.z/100, 0, shift.y/100);
            Debug.Log("shift: " + shift + ", newCityPos: " + City.transform.position);
        }
	}

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "ScaleRotateExampleScene")
        {
            City = GameObject.Find("SoftwareCity");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Head")
        {
            Debug.Log("OnTriggerEnter MiniMap: " + other.gameObject.name);
            triggerPos = other.transform.position;
            controllerEvents = other.gameObject.GetComponentInParent<VRTK_ControllerEvents>();
            if (controllerEvents)
            {
                controllerEvents.AliasGrabOn += ControllerEvents_AliasGrabOn;
                controllerEvents.AliasGrabOff += ControllerEvents_AliasGrabOff;
                Debug.Log("Grabs hinzugefügt");
            }
        }
    }

    private void ControllerEvents_AliasGrabOff(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("grab off");
        isGrabbed = false;
    }

    private void ControllerEvents_AliasGrabOn(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("grab on");
        isGrabbed = true;
    }

    private void OnTriggerStay(Collider other)
    {
        oldPos = triggerPos;
        triggerPos = other.transform.position;
        isTriggered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Head")
        {
            isTriggered = false;
            controllerEvents.AliasGrabOn -= ControllerEvents_AliasGrabOn;
            controllerEvents.AliasGrabOff -= ControllerEvents_AliasGrabOff;
        }
    }
}
