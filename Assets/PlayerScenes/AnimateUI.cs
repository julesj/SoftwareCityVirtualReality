using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateUI : MonoBehaviour {
    private Vector3 newPosTop;
    private Vector3 newPosBottom;
    private Vector3 oldPosTop;
    private Vector3 oldPosBottom;

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
        AnimateThis.With(this).Transformate().
                    ToPosition(newPosTop)
                    .Duration(1)
                    .Ease(AnimateThis.EaseInOutSmooth)
                    .Start();
    }

    public void SwipeTopUp()
    {
        AnimateThis.With(this).Transformate().
                    ToPosition(oldPosTop)
                    .Duration(1)
                    .Ease(AnimateThis.EaseInOutSmooth)
                    .Start();
    }

    public void SwipeBottomUp()
    {
        AnimateThis.With(this).Transformate().
                    ToPosition(newPosBottom)
                    .Duration(1)
                    .Ease(AnimateThis.EaseInOutSmooth)
                    .Start();
    }

    public void SwipeBottomDown()
    {
        AnimateThis.With(this).Transformate().
                    ToPosition(oldPosBottom)
                    .Duration(1)
                    .Ease(AnimateThis.EaseInOutSmooth)
                    .Start();
    }
}
