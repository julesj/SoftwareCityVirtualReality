using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SwipeDetect : MonoBehaviour
{
    private Vector2 axis;
    private bool checkSwipe;
    private Vector2 startPos;
    private Vector2 endPos;
    private float startTime;
    private float endTime;

    private readonly Vector2 xAxis = new Vector2(1, 0);
    private readonly Vector2 yAxis = new Vector2(0, 1);

    public float minVelocity = 2.0f;
    public float minSwipeDist = 0.5f;

    // Use this for initialization
    void Start()
    {
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);
    }

    // Update is called once per frame
    void Update()
    {
        if(checkSwipe)
        {

        }
    }

    void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("TouchStart " + e.touchpadAxis);
        startPos = e.touchpadAxis;
        startTime = Time.time;
    }

    void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log("TouchReleased " + e.touchpadAxis);
        endPos = e.touchpadAxis;
        endTime = Time.time;
        CheckSwipe();
    }

    void CheckSwipe()
    {
        float deltaTime = endTime - startTime;
        Vector2 swipeVector = endPos - startPos;
        float velocity = swipeVector.magnitude / deltaTime;
        Debug.Log("magnitude: " + swipeVector.magnitude);
        Debug.Log("Velocity: " + velocity);

        if (velocity > minVelocity && swipeVector.magnitude > minSwipeDist)
        {
            float angleOfSwipe = Mathf.Atan2(swipeVector.y, swipeVector.x) * Mathf.Rad2Deg ;
            Debug.Log("angle: " + angleOfSwipe); //Winkel geht zwischen 180° (oben) und -180° (unten)
            //if (angleOfSwipe < ...) {swipeUI down -> evtl mit AnimateThis zusammen}
        }
    }
}
