using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class ControllerScaleRotateBar : MonoBehaviour
{
    private FloatModel scaleModel;
    private FloatModel rotateModel;
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    private Vector2 axis;
    private float angle;
    private ClipboardBar clipboard;
    private bool doScale;
    private bool doRotate;
    private GrowBar growBar;

    public float scaleDelta = 0.005f;
    public float rotateDelta = 0.005f;
    public float valueBeginOfRotateAboutUser = 0.8f;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        growBar = GameObject.Find("SoftwareCity").GetComponent<GrowBar>();
    }

    private void Update()
    {
        if (doScale && scaleModel)
        {
            float value = scaleModel.GetValue();
            if (angle < 45 || angle > 315)
            {
                scaleModel.SetValue(value + scaleDelta, true);
            }
            else if (angle < 225 && angle > 135)
            {
                scaleModel.SetValue(value - scaleDelta, true);
            }
        }

        if (doRotate && rotateModel && scaleModel)
        {
            float value = rotateModel.GetValue();

            if (scaleModel.GetValue() > valueBeginOfRotateAboutUser)
            {//Rotate about User
                if (angle < 135 && angle > 45)
                {
                    growBar.SetRotationValue(rotateDelta, true);
                } else
                {
                    growBar.SetRotationValue(-rotateDelta, true);
                }
                    
            }
            else
            {//Rotate about City
                if (angle < 135 && angle > 45)
                {
                    if (value == 1)
                    {
                        rotateModel.SetValue(0f + rotateDelta, false);
                    }
                    else
                    {
                        rotateModel.SetValue(value + rotateDelta, false);
                    }
                }
                else if (angle > 225 && angle < 315)
                {
                    if (value == 0)
                    {
                        rotateModel.SetValue(1f - rotateDelta, false);
                    }
                    else
                    {
                        rotateModel.SetValue(value - rotateDelta, false);
                    }
                }
            }
        }
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Szene " + scene.name + " wurde geladen");
        if (scene.name.Equals(ClipboardBar.LoadableScenes.ScaleRotateExampleScene.ToString()))
        {
            //GetComponent<GoBack>().enabled = false;
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
            GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);
            GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
            GetComponent<VRTK_ControllerEvents>().TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);
            clipboard = GameObject.Find("Main").GetComponent<ClipboardBar>();
        }
    }

    void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        axis = e.touchpadAxis;
        angle = e.touchpadAngle;
    }

    void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        doScale = true;
        doRotate = true;
    }

    void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        doScale = false;
        doRotate = false;
    }
}
