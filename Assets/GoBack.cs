using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VRTK
{
    //Dieses Skript muss auf einem Controller liegen!
    public class GoBack : MonoBehaviour
    {
        
        SteamVR_TrackedObject trackedObj;
        SteamVR_Controller.Device device;
        Vector2 axis;
        float angle;
        ClipboardBar clipboard;

        // Use this for initialization
        void Awake()
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        void Start()
        {
            GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);
            GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
            clipboard = GameObject.Find("Main").GetComponent<ClipboardBar>();
        }

        void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
        {
            axis = e.touchpadAxis;
            angle = e.touchpadAngle;
        }

        void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
        {
            if (angle > 180)
            {
                Debug.Log("before: " + clipboard.getBefore());
                SceneManager.LoadScene(clipboard.getBefore());
            }
        }
    }
}
