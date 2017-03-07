using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class ControllerRotateBar : MonoBehaviour
{
    private FloatModel rotateModel;
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    private Vector2 axis;
    private float angle;
    private ClipboardBar clipboard;
    private bool doRotate;

    public float rotateDelta = 0.01f;

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void Update()
    {
        if (doRotate)
        {
            float value = rotateModel.GetValue();
            if (angle < 180)
            {
                if (value == 1)
                {
                    rotateModel.SetValue(0f + rotateDelta);
                }
                else
                {
                    rotateModel.SetValue(value + rotateDelta);
                }
            }
            else if (angle > 180)
            {
                if (value == 0)
                {
                    rotateModel.SetValue(1f - rotateDelta);
                }
                else
                {
                    rotateModel.SetValue(value - rotateDelta);
                }
            }
        }
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Szene " + scene.name + " wurde geladen");
        if (scene.name == "ScaleRotateExampleScene")
        {
            foreach (FloatModel model in FindObjectsOfType<FloatModel>())
            {
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
        doRotate = true;
    }

    void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        doRotate = false;
    }
}

