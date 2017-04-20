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
    bool isController;
    VRTK_ControllerEvents controllerEvents;
    GameObject City;
    GameObject Minimap;
    Vector3 fingerpos;
    Vector3 oldPos1;
    Vector3 oldPos2;
    FloatModel scaleModel;
    FloatModel rotateModel;
    LeapServiceProvider provider;

    public int scaleFactorMove = 10;
    public float scaleFactorScaleRotate = 10f;

    void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        provider = FindObjectOfType<LeapServiceProvider>();
    }

    // Update is called once per frame
    void Update () {
        if (isGrabbed && !secondGrab)
        {
            MoveCity();
            FindObjectOfType<HelpSystemMixed>().Deactivate();
        } else if (isGrabbed && secondGrab)
        {
            ScaleRotateCity();
            FindObjectOfType<HelpSystemMixed>().Deactivate();
        }
	}

    void ScaleRotateCity()
    {
        Hand hand1 = provider.CurrentFrame.Hands[0];
        Vector3 pos1 = hand1.PalmPosition.ToVector3();
        Vector3 pos2 = gameObject.transform.position;
        Vector3 actual = Minimap.transform.InverseTransformDirection(pos1 - pos2);
        Vector3 before = Minimap.transform.InverseTransformDirection(oldPos1 - oldPos2);

        float angle = Mathf.Atan2(actual.y, actual.x) - Mathf.Atan2(before.y, before.x);
        Debug.Log("rotate around " + angle);
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
        if (isController)
        {
            triggerPos = gameObject.transform.position;
        } else
        {
            GetFingersPos();
            triggerPos = fingerpos;
        }
        
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
            Debug.Log("OnTriggerEnterMinimapHandler: " + other.name);
            controllerEvents = gameObject.GetComponentInParent<VRTK_ControllerEvents>();
            controllerEvents.TriggerPressed -= ControllerEvents_AliasGrabOn;
            controllerEvents.TriggerReleased -= ControllerEvents_AliasGrabOff;
            controllerEvents.TriggerPressed += ControllerEvents_AliasGrabOn;
            controllerEvents.TriggerReleased += ControllerEvents_AliasGrabOff;
            gameObject.GetComponent<VRTK_ControllerActions>().ToggleHighlightTrigger(true, new Color(192,190,255), 0.5f);
            SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(2000, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
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
        controllerEvents.TriggerPressed -= ControllerEvents_AliasGrabOn;
        controllerEvents.TriggerReleased -= ControllerEvents_AliasGrabOff;
        isController = false;
    }

    private void ControllerEvents_AliasGrabOn(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("grab on");
        triggerPos = gameObject.transform.position;
        oldPos2 = gameObject.transform.position;
        if (isGrabbed)
        {
            secondGrab = true;
        } else
        {
            isGrabbed = true;
        }
        isController = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Cube_Minimap" && !isGrabbed)
        {
            controllerEvents.TriggerPressed -= ControllerEvents_AliasGrabOn;
            controllerEvents.TriggerReleased -= ControllerEvents_AliasGrabOff;
            gameObject.GetComponent<VRTK_ControllerActions>().ToggleHighlightTrigger(false);
        }
    }

    public void SetIsGrapped(bool grapped)
    {
        if (Minimap != null)
        { 
            GetFingersPos();
            Vector3 relPos = Minimap.transform.InverseTransformPoint(fingerpos);
            if (!grapped)
            {
                if (secondGrab)
                {
                    Debug.Log("Set Secondgrab false");
                    secondGrab = false;
                }
                else if (isGrabbed)
                {
                    Debug.Log("Set isGrabbed false");
                    isGrabbed = false;
                }
            } else if (grapped && Mathf.Abs(relPos.z) < 50)
            {
                if (isGrabbed)
                {
                    Debug.Log("Set Secondgrab true");
                    secondGrab = true;
                } else if (!isGrabbed)
                {
                    Debug.Log("Set isGrabbed true");
                    isGrabbed = true;
                }
                triggerPos = fingerpos;
                oldPos1 = fingerpos;
            }
        }
    }

    private void GetFingersPos()
    {
        List<Hand> hands = provider.CurrentFrame.Hands;
        if (hands.Count > 0)
        {
            Hand hand = hands[0];
            List<Finger> fingerList = hand.Fingers;
            foreach (Finger finger in fingerList)
            {
                if (finger.Type == Finger.FingerType.TYPE_INDEX)
                {
                    fingerpos = finger.TipPosition.ToVector3();
                }
            }
        }
        
    }
}
