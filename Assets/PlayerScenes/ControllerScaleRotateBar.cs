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
    private Vector2 axis;
    private float angle;
    private ClipboardBar clipboard;
    private bool doScale;
    private bool doRotate;
    private GrowBar growBar;
    private bool rotateAboutUser;

    public float scaleDelta = 0.005f;
    public float rotateDelta = 0.005f;
    public float valueBeginOfRotateAboutUser = 0.8f;

    void Awake()
    {
        //trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void Update()
    {
        if (doScale && scaleModel)
        {//Scale always about User
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
            {
                rotateAboutUser = true;
            } else
            {
                rotateAboutUser = false;
            }
           
            if (angle < 135 && angle > 45)
            {
                if (value == 1)
                {
                    rotateModel.SetValue(0f + rotateDelta, rotateAboutUser);
                }
                else
                {
                    rotateModel.SetValue(value + rotateDelta, rotateAboutUser);
                }
            }
            else if (angle > 225 && angle < 315)
            {
                if (value == 0)
                {
                    rotateModel.SetValue(1f - rotateDelta, rotateAboutUser);
                }
                else
                {
                    rotateModel.SetValue(value - rotateDelta, rotateAboutUser);
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
            growBar = GameObject.Find("SoftwareCity").GetComponent<GrowBar>();
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
