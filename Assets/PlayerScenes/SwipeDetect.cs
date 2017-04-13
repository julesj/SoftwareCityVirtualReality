using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SwipeDetect : MonoBehaviour
{//auf Controller legen
    private Vector2 startPos;
    private Vector2 endPos;
    private float startTime;
    private float endTime;

    private AnimateUI scaleRotate;

    private readonly Vector2 xAxis = new Vector2(1, 0);
    private readonly Vector2 yAxis = new Vector2(0, 1);

    public float minVelocity = 2.0f;
    public float minSwipeDist = 0.5f;

    // Use this for initialization
    void Start()
    {
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);
        scaleRotate = GameObject.Find("Canvas_Zoom_Rotate").GetComponent<AnimateUI>();
    }

    void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        startPos = e.touchpadAxis;
        startTime = Time.time;
    }

    void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        endPos = e.touchpadAxis;
        endTime = Time.time;
        CheckSwipe();
    }

    void CheckSwipe()
    {
        float deltaTime = endTime - startTime;
        Vector2 swipeVector = endPos - startPos;
        float velocity = swipeVector.magnitude / deltaTime;

        if (velocity > minVelocity && swipeVector.magnitude > minSwipeDist)
        {
            float angleOfSwipe = Mathf.Atan2(swipeVector.y, swipeVector.x) * Mathf.Rad2Deg ;
            Debug.Log("angle: " + angleOfSwipe); //Winkel geht zwischen 180° und -180° (relativ zur x-Achse)
            
            bool scaleRotateVisibleTop = scaleRotate.GetIsVisbleTop();
            bool scaleRotateVisibleBottom = scaleRotate.GetIsVisibleBottom();
            if (angleOfSwipe < -75 && angleOfSwipe > -105)
            {
                if (!scaleRotateVisibleTop && !scaleRotateVisibleBottom)
                {
                    TopDown();
                } else if (scaleRotateVisibleBottom)
                {
                    BottomDown();
                }
            } else if (angleOfSwipe > 75 && angleOfSwipe < 105)
            {
                if (scaleRotateVisibleTop)
                {
                    TopUp();
                }
                else if (!scaleRotateVisibleBottom && !scaleRotateVisibleTop)
                {
                    BottomUp();
                }
            }
        }
    }

    void TopUp()
    {
        Debug.Log("should swipe topup");
        scaleRotate.SwipeTopUp();
    }

    void TopDown()
    {
        Debug.Log("should swipe topdown");
        scaleRotate.SwipeTopDown();
    }

    void BottomUp()
    {
        Debug.Log("should swipe bottomup");
        scaleRotate.SwipeBottomUp();
    }

    void BottomDown()
    {
        Debug.Log("should swipe bottomdown");
        scaleRotate.SwipeBottomDown();
    }
}
