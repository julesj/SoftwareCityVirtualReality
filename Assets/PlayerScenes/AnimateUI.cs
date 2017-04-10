using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateUI : MonoBehaviour {
    private Vector3 newPosTop;
    private Vector3 newPosBottom;
    private Vector3 oldPosTop;
    private Vector3 oldPosBottom;

    private Transform oldParent;

    public bool isVisibleTop = false;
    public bool isVisibleBottom = false;

    private void Start()
    {
        if (gameObject.name.Equals("Canvas_Rotate"))
        {
            newPosTop = new Vector3(-0.5f, 0.25f, 0.9f);
            newPosBottom = new Vector3(-0.5f, -0.3f, 0.9f);
            oldPosTop = new Vector3(-0.1f, 0.8f, 1.4f);
            oldPosBottom = new Vector3(-0.1f, -0.8f, 1.4f);
        }
        else if (gameObject.name.Equals("Canvas_Zoom"))
        {
            newPosTop = new Vector3(0.3f, 0.25f, 0.9f);
            newPosBottom = new Vector3(0.3f, -0.3f, 0.9f);
            oldPosTop = new Vector3(0.8f, 0.8f, 1.4f);
            oldPosBottom = new Vector3(0.8f, -0.8f, 1.4f);
        }
    }

    public void SwipeTopDown()
    {
        if (gameObject.transform.localPosition != oldPosTop)
        {
            gameObject.transform.localPosition = oldPosTop;
        }
        AnimateThis.With(this).Transformate().
                    ToPosition(newPosTop)
                    .Duration(1)
                    .Ease(AnimateThis.EaseInOutSmooth)
                    .Start();
        isVisibleTop = true;
        oldParent = gameObject.transform.parent;
        //Wait for Animation Ending!
        //gameObject.transform.parent = null;
    }

    public void SwipeTopUp()
    {
        //gameObject.transform.parent = oldParent;
        AnimateThis.With(this).Transformate().
                    ToPosition(oldPosTop)
                    .Duration(1)
                    .Ease(AnimateThis.EaseInOutSmooth)
                    .Start();
        isVisibleTop = false;
    }

    public void SwipeBottomUp()
    {
        if (gameObject.transform.localPosition != oldPosBottom)
        {
            gameObject.transform.localPosition = oldPosBottom;
        }
        AnimateThis.With(this).Transformate().
                    ToPosition(newPosBottom)
                    .Duration(1)
                    .Ease(AnimateThis.EaseInOutSmooth)
                    .Start();
        isVisibleBottom = true;
        oldParent = gameObject.transform.parent;
        //gameObject.transform.parent = null;
    }

    public void SwipeBottomDown()
    {
        //gameObject.transform.parent = oldParent;
        AnimateThis.With(this).Transformate().
                    ToPosition(oldPosBottom)
                    .Duration(1)
                    .Ease(AnimateThis.EaseInOutSmooth)
                    .Start();
        isVisibleBottom = false;
    }
}
