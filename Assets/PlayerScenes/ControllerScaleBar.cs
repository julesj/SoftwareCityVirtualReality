using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.SceneManagement;

public class ControllerScaleBar : MonoBehaviour {
    private FloatModel scaleModel;
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device device;
    private Vector2 axis;
    private float angle;
    private ClipboardBar clipboard;
    private bool doScale;

    public float scaleDelta = 0.005f;

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
        if (doScale)
        {
            float value = scaleModel.GetValue();
            if (angle < 90 || angle > 270)
            {
                scaleModel.SetValue(value + scaleDelta);
            }
            else if (angle < 270 && angle > 90)
            {
                scaleModel.SetValue(value - scaleDelta);
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
                if (model.name.Equals("Scale"))
                {
                    scaleModel = model;
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
    }

    void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        doScale = false;
    }
}
