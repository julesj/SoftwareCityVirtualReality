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
    bool isGrabbed;
    VRTK_ControllerEvents controllerEvents;
    GameObject City;

    uint controllerIndex1;

    public int scaleFactor = 10;

    void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    // Update is called once per frame
    void Update () {
        //GameObject controller = VRTK_DeviceFinder.GetControllerByIndex(controllerIndex1, false);
        //Debug.Log("ControllerIndex: " + controllerIndex1);
        //Debug.Log("Controller Pos Update x: " + controller.transform.position);

        if (isGrabbed)
        {
            
            //if (controller)
            //{
            //    Debug.Log("Controller Pos Update x: " + controller.transform.position.x);

                oldPos = triggerPos;
                triggerPos = gameObject.transform.position;

                Debug.Log("oldPos: x=" + oldPos.x + ", y=" + oldPos.y + ", z=" + oldPos.z);
                Debug.Log("newPos: x=" + triggerPos.x + ", y=" + triggerPos.y + ", z=" + triggerPos.z);
                Vector3 shift = triggerPos - oldPos; //Global? -> y & z; für City z & x
                City.transform.position += new Vector3(shift.z * scaleFactor, 0, -shift.y * scaleFactor);
                Debug.Log("shift: " + shift + ", newCityPos: " + City.transform.position);
            //} else
            //{
            //    Debug.Log("!!!!!!!!!!!!!!!!!!  Controller is null");
            //}

            
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
        
        if (other.gameObject.name == "Cube_Minimap")
        {
            Debug.Log("OnTriggerEnter MiniMap: " + other.gameObject.name);
            controllerEvents = gameObject.GetComponentInParent<VRTK_ControllerEvents>();

            //GameObject controller = controllerEvents.gameObject;
            
            //if (controller)
            //{
            //    controllerIndex1 = VRTK_DeviceFinder.GetControllerIndex(controller);
            //    Debug.Log("position x OnTriggerEnter: " + controller.transform.position.x);

                controllerEvents.GripPressed += ControllerEvents_AliasGrabOn;
                controllerEvents.GripReleased += ControllerEvents_AliasGrabOff;
                Debug.Log("Grabs hinzugefügt");
            //}
        }
    }

    private void ControllerEvents_AliasGrabOff(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("grab off");
        isGrabbed = false;
        controllerEvents.GripPressed -= ControllerEvents_AliasGrabOn;
        controllerEvents.GripReleased -= ControllerEvents_AliasGrabOff;
    }

    private void ControllerEvents_AliasGrabOn(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("grab on");
        //GameObject controller = VRTK_DeviceFinder.GetControllerByIndex(controllerIndex1, false);
        //if (controller)
        //{
            triggerPos = gameObject.transform.position;
        //}
        isGrabbed = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Cube_Minimap" && !isGrabbed)
        {
            Debug.Log("OnTriggerExit");
            controllerEvents.GripPressed -= ControllerEvents_AliasGrabOn;
            controllerEvents.GripReleased -= ControllerEvents_AliasGrabOff;
        }
    }
}
