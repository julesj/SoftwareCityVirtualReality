using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
using VRTK;
using Leap.Unity;
using Leap;

public class MiniMapHandler : MonoBehaviour {

    Vector3 triggerPos;
    Vector3 oldPos;
    bool isGrabbed;
    VRTK_ControllerEvents controllerEvents;
    GameObject City;
    GameObject Minimap;

    uint controllerIndex1;

    public int scaleFactor = 10;

    void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    // Update is called once per frame
    void Update () {

        if (isGrabbed)
        {
            oldPos = triggerPos;
            triggerPos = gameObject.transform.position;

            Debug.Log("oldPos: x=" + oldPos.x + ", y=" + oldPos.y + ", z=" + oldPos.z);
            Debug.Log("newPos: x=" + triggerPos.x + ", y=" + triggerPos.y + ", z=" + triggerPos.z);
            Vector3 shift = triggerPos - oldPos; //Global? -> y & z; für City z & x
            Vector3 shiftLocal = Minimap.gameObject.transform.InverseTransformDirection(shift);
            City.transform.position += new Vector3(shiftLocal.x * scaleFactor, 0, shiftLocal.y * scaleFactor);
            Debug.Log("shift: " + shift + ", newCityPos: " + City.transform.position);
        }
	}

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals(ClipboardBar.LoadableScenes.ScaleRotateExampleScene.ToString()))
        {
            City = GameObject.Find("SoftwareCity");
            Minimap = GameObject.Find("Cube_Minimap");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.name == "Cube_Minimap")
        {
            Debug.Log("OnTriggerEnter MiniMap: " + other.gameObject.name);
            controllerEvents = gameObject.GetComponentInParent<VRTK_ControllerEvents>();
            controllerEvents.TriggerPressed += ControllerEvents_AliasGrabOn;
            controllerEvents.TriggerReleased += ControllerEvents_AliasGrabOff;
            Debug.Log("Grabs hinzugefügt");
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
        triggerPos = gameObject.transform.position;
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

    public void SetIsGrapped(bool grapped)
    {
        

        isGrabbed = grapped;
        triggerPos = gameObject.transform.position;
    }
}
