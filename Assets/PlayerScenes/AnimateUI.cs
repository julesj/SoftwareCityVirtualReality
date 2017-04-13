using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRTK;

public class AnimateUI : MonoBehaviour {
    private Vector3 newPosTop;
    private Vector3 newPosBottom;
    private Vector3 oldPosTop;
    private Vector3 oldPosBottom;
    public float newPosUpFactor = 0.5f;
    public float newPosForwardFactor = 0.5f;

    private Transform oldParent;
    private float[] factors;
    private Transform headsetTransform;
    private PositionToHeadset posToHead;

    private bool isVisibleTop = false;
    private bool isVisibleBottom = false;
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
        newPosBottom = oldPosBottom + headsetTransform.up * newPosUpFactor + headsetTransform.forward * newPosForwardFactor;
        oldPosBottom = headsetTransform.position + headsetTransform.right * factors[0] - headsetTransform.up * factors[1] + headsetTransform.forward * factors[2];
        //gameObject.transform.LookAt(headsetTransform);
        gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, 0);
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

    public void SetIsVisibleBottom(bool isVisible)
    {
        isVisibleBottom = isVisible;
    }

    public bool GetIsVisibleBottom()
    {
        return isVisibleBottom;
    }

    public void SetIsVisibleTop(bool isVisible)
    {
        isVisibleTop = isVisible;
    }

    public bool GetIsVisbleTop()
    {
        return isVisibleTop;
    }
}
