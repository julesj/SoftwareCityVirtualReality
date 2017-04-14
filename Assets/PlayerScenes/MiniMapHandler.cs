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
    //Dieses Skript liegt auf dem Controller
    Vector3 triggerPos;
    Vector3 oldPos;
    bool isGrabbed; //für Controller und Hand
    bool secondGrab;
    VRTK_ControllerEvents controllerEvents;
    GameObject City;
    GameObject Minimap;
    Vector3 oldPos1;
    Vector3 oldPos2;
    FloatModel scaleModel;
    FloatModel rotateModel;
    LeapServiceProvider provider;

    public int scaleFactorMove = 10;
    public float scaleFactorScaleRotate = 0.5f;

    void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        
    }

    // Update is called once per frame
    void Update () {

        if (isGrabbed && !secondGrab)
        {
            MoveCity();
        } else if (isGrabbed && secondGrab)
        {
            ScaleRotateCity();
        }
	}

    void ScaleRotateCity()
    {
        Hand hand1 = provider.CurrentFrame.Hands[0];
        Vector3 pos1 = hand1.PalmPosition.ToVector3();
        Vector3 pos2 = gameObject.transform.position;
        Vector3 actual = Minimap.transform.InverseTransformDirection(pos1 - pos2);
        Vector3 before = Minimap.transform.InverseTransformDirection(oldPos1 - oldPos2);

        float angle = Mathf.Atan2(actual.z, actual.x) - Mathf.Atan2(before.z, before.x);
        float actRotateValue = rotateModel.GetValue();
        if (actRotateValue == 0.0f && angle * Mathf.Rad2Deg > 0)
        {
            rotateModel.SetValue(1.0f - Mathf.Rad2Deg * angle / 360);
        }
        else if (actRotateValue == 1.0f && angle * Mathf.Rad2Deg < 0)
        {
            rotateModel.SetValue(0.0f - Mathf.Rad2Deg * angle / 360);
        }
        else
        {
            rotateModel.SetValue(actRotateValue - Mathf.Rad2Deg * angle / 360);
        }

        float diffLength = actual.magnitude - before.magnitude;
        float actScaleValue = scaleModel.GetValue();
        scaleModel.SetValue(actScaleValue + diffLength * scaleFactorScaleRotate);

        oldPos1 = pos1;
        oldPos2 = pos2;
    }

    void MoveCity()
    {
        oldPos = triggerPos;
        triggerPos = gameObject.transform.position;
        Vector3 shift = triggerPos - oldPos; //Global? -> y & z; für City z & x
        Vector3 shiftLocal = Minimap.gameObject.transform.InverseTransformDirection(shift);
        City.transform.position += new Vector3(shiftLocal.x * scaleFactorMove, 0, shiftLocal.y * scaleFactorMove);
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals(ClipboardBar.LoadableScenes.ScaleRotateExampleScene.ToString()))
        {
            City = GameObject.Find("SoftwareCity");
            Minimap = GameObject.Find("Cube_Minimap");

            foreach (FloatModel model in FindObjectsOfType<FloatModel>())
            {
                if (model.name.Equals("Scale"))
                {
                    scaleModel = model;
                }
                if (model.name.Equals("Rotate"))
                {
                    rotateModel = model;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Cube_Minimap")
        {
            controllerEvents = gameObject.GetComponentInParent<VRTK_ControllerEvents>();
            controllerEvents.TriggerPressed += ControllerEvents_AliasGrabOn;
            controllerEvents.TriggerReleased += ControllerEvents_AliasGrabOff;
        }
    }

    private void ControllerEvents_AliasGrabOff(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("grab off");
        if (secondGrab)
        {
            secondGrab = false;
        } else
        {
            isGrabbed = false;
        }
        controllerEvents.GripPressed -= ControllerEvents_AliasGrabOn;
        controllerEvents.GripReleased -= ControllerEvents_AliasGrabOff;
    }

    private void ControllerEvents_AliasGrabOn(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("grab on");
        triggerPos = gameObject.transform.position;
        if (isGrabbed)
        {
            secondGrab = true;
        } else
        {
            isGrabbed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Cube_Minimap" && !isGrabbed)
        {
            controllerEvents.GripPressed -= ControllerEvents_AliasGrabOn;
            controllerEvents.GripReleased -= ControllerEvents_AliasGrabOff;
        }
    }

    public void SetIsGrapped(bool grapped)
    {
        if (Minimap)
        {//geht das mit gameobject oder muss Hand gesucht werden?
            Vector3 relPos = Minimap.transform.InverseTransformPoint(gameObject.transform.position);
            if (Mathf.Abs(relPos.z) < 20)
            {
                if (isGrabbed && grapped)
                {
                    secondGrab = true;
                } else if (!isGrabbed && grapped)
                {
                    isGrabbed = true;
                } else if (secondGrab && !grapped)
                {
                    secondGrab = false;
                } else if (isGrabbed && !grapped)
                {
                    isGrabbed = false;
                }
                
                triggerPos = gameObject.transform.position;
            }
        }
    }
}
