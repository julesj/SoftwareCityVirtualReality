using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class AnimateUI : MonoBehaviour {
    private Vector3 newPosTop;
    private Vector3 newPosBottom;
    private Vector3 newPosRight;
    private Vector3 newPosLeft;
    private Vector3 oldPosTop;
    private Vector3 oldPosBottom;
    private Vector3 oldPosRight;
    private Vector3 oldPosLeft;
    public float newPosUpFactor = 0.5f;
    public float newPosForwardFactor = 0.5f;
    public float newPosRightFactor = 0.5f;

    private Transform oldParent;
    private float[] factors;
    private Transform headsetTransform;
    private PositionToHeadset posToHead;

    private bool isVisibleTop = false;
    private bool isVisibleBottom = false;
    private bool isVisibleRight = false;
    private bool isVisibleLeft = false;
    private bool cityStarted = false;

    private void Start()
    {
        posToHead = GetComponent<PositionToHeadset>();
        factors = posToHead.GetFactors();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name.Equals(ClipboardBar.LoadableScenes.ScaleRotateExampleScene.ToString()))
        {
            cityStarted = true;
        }
    }

    private void Update()
    {
        headsetTransform = VRTK_DeviceFinder.HeadsetTransform();

        oldPosTop = headsetTransform.position + headsetTransform.right * factors[0] + headsetTransform.up * factors[1] + headsetTransform.forward * factors[2];
        newPosTop = oldPosTop - headsetTransform.up * newPosUpFactor + headsetTransform.forward * newPosForwardFactor;
        
        oldPosBottom = headsetTransform.position + headsetTransform.right * factors[0] - headsetTransform.up * factors[1] + headsetTransform.forward * factors[2];
        newPosBottom = oldPosBottom + headsetTransform.up * newPosUpFactor + headsetTransform.forward * newPosForwardFactor;

        oldPosRight = headsetTransform.position + headsetTransform.right * factors[0] + headsetTransform.up * factors[1] + headsetTransform.forward * factors[2];
        newPosRight = oldPosRight + headsetTransform.forward * newPosForwardFactor - headsetTransform.right * newPosRightFactor;

        oldPosLeft = headsetTransform.position - headsetTransform.right * factors[0] + headsetTransform.up * factors[1] + headsetTransform.forward * factors[2];
        newPosLeft = oldPosLeft + headsetTransform.forward * newPosForwardFactor + headsetTransform.right * newPosRightFactor;

        gameObject.transform.LookAt(headsetTransform);
    }

    public void SwipeTopDown()
    {
        if (cityStarted)
        {
            posToHead.followHeadset = false;
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
        }
    }

    public void SwipeTopUp()
    {
        if (cityStarted)
        {
            AnimateThis.With(this).Transformate().
                        ToPosition(oldPosTop)
                        .Duration(1)
                        .Ease(AnimateThis.EaseInOutSmooth)
                        .Start();
            isVisibleTop = false;
            posToHead.followHeadset = true;
        }
    }

    public void SwipeBottomUp()
    {
        if (cityStarted)
        {
            posToHead.followHeadset = false;
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
        }
    }

    public void SwipeBottomDown()
    {
        if (cityStarted)
        {
            AnimateThis.With(this).Transformate().
                        ToPosition(oldPosBottom)
                        .Duration(1)
                        .Ease(AnimateThis.EaseInOutSmooth)
                        .Start();
            isVisibleBottom = false;
            posToHead.followHeadset = true;
        }
    }

    public void SwipeRightIn()
    {
        if (cityStarted)
        {
            posToHead.followHeadset = false;
            if (gameObject.transform.localPosition != oldPosRight)
            {
                gameObject.transform.localPosition = oldPosRight;
            }
            AnimateThis.With(this).Transformate().
                ToPosition(newPosRight)
                .Duration(1)
                .Ease(AnimateThis.EaseInOutSmooth)
                .Start();
            isVisibleRight = true;
        }
    }

    public void SwipeRightOut()
    {
        if (cityStarted)
        {
            AnimateThis.With(this).Transformate().
                        ToPosition(oldPosRight)
                        .Duration(1)
                        .Ease(AnimateThis.EaseInOutSmooth)
                        .Start();
            isVisibleRight = false;
            posToHead.followHeadset = true;
        }
    }

    public void SwipeLeftIn()
    {
        if (cityStarted)
        {
            posToHead.followHeadset = false;
            if (gameObject.transform.localPosition != oldPosLeft)
            {
                gameObject.transform.localPosition = oldPosLeft;
            }
            AnimateThis.With(this).Transformate().
                ToPosition(newPosLeft)
                .Duration(1)
                .Ease(AnimateThis.EaseInOutSmooth)
                .Start();
            isVisibleLeft = true;
        }
    }

    public void SwipeLeftOut()
    {
        if (cityStarted)
        {
            AnimateThis.With(this).Transformate().
                        ToPosition(oldPosLeft)
                        .Duration(1)
                        .Ease(AnimateThis.EaseInOutSmooth)
                        .Start();
            isVisibleLeft = false;
            posToHead.followHeadset = true;
        }
    }

    public bool GetIsVisibleBottom()
    {
        return isVisibleBottom;
    }

    public bool GetIsVisbleTop()
    {
        return isVisibleTop;
    }

    public bool GetIsVisibleRight()
    {
        return isVisibleRight;
    }

    public bool GetIsVisibleLeft()
    {
        return isVisibleLeft;
    }
}
