using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ControlKnob : MonoBehaviour {
    private bool doRotate;
    private LeapServiceProvider provider;
    private FloatModel rotateModel;
    private FloatModel scaleModel;
    private bool cityLoaded;

    private Vector3 actPos;
    private Vector3 beforePos;

    public float negativBorder = -20;
    public float positivBorder = 10;
    public Text write;

	// Use this for initialization
	void Start () {
        provider = FindObjectOfType<LeapServiceProvider>();
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name.Equals(ClipboardBar.LoadableScenes.ScaleRotateExampleScene.ToString()))
        {
            foreach (FloatModel model in FindObjectsOfType<FloatModel>())
            {
                if (model.name.Equals("Rotate"))
                {
                    rotateModel = model;
                }
                if (model.name.Equals("Scale"))
                {
                    scaleModel = model;
                }
            }
            cityLoaded = true;
        }
    }

    // Update is called once per frame
    void Update () {
        if (cityLoaded)
        {
            if (provider.CurrentFrame.Hands.Count > 0)
            {
                GetFingersPos();
            }

            Vector3 actPosLocal = gameObject.transform.InverseTransformPoint(actPos);
            if (actPosLocal.z > negativBorder && actPosLocal.z < positivBorder && Mathf.Abs(actPosLocal.x) < 90 && Mathf.Abs(actPosLocal.y) < 90) 
            {
                doRotate = true;
            }
            else
            {
                doRotate = false;
                beforePos = Vector3.zero;
            }

            if (doRotate)
            {
                if (actPos != null)
                {
                    float angle = GetRotateAngle();
                    rotateArrow(angle * Mathf.Rad2Deg);
                    rotateCity(angle * Mathf.Rad2Deg);
                    WriteText(angle * Mathf.Rad2Deg / 3.6f);
                    beforePos = actPos;

                    HelpSystemMixed help = FindObjectOfType<HelpSystemMixed>();
                    if (help.processed[1] && !help.processed[2])
                    {
                        help.Deactivate();
                    }
                }
            }
        }
	}

    private void GetFingersPos()
    {
        Hand hand = provider.CurrentFrame.Hands[0];
        List<Finger> fingerList = hand.Fingers;
        foreach (Finger finger in fingerList)
        {
            if (finger.Type == Finger.FingerType.TYPE_INDEX)
            {
                actPos = finger.TipPosition.ToVector3();
            }
        }
    }

    private float GetRotateAngle()
    {
        if (beforePos != null && beforePos != Vector3.zero)
        {
            Vector3 actDirection = actPos - gameObject.transform.position;
            Vector3 beforeDirection = beforePos - gameObject.transform.position;
            Vector3 actDirLocal = gameObject.transform.InverseTransformDirection(actDirection);
            Vector3 beforeDirLocal = gameObject.transform.InverseTransformDirection(beforeDirection);
            float angle = Mathf.Atan2(actDirLocal.y, actDirLocal.x) - Mathf.Atan2(beforeDirLocal.y, beforeDirLocal.x);
            Debug.Log("angle: " + angle * Mathf.Rad2Deg);
            return angle;
        }
        return 0;
    }

    private void rotateArrow(float angle)
    {
        GameObject arrow = GameObject.Find("Image_Arrow").gameObject;
        arrow.transform.RotateAround(gameObject.transform.position, gameObject.transform.forward, angle);
    }

    private void rotateCity(float angle)
    {//hier Änderungen
        if (rotateModel && scaleModel)
        {
            float actValue = rotateModel.GetValue();
            if (scaleModel.GetValue() > 0.8f)
            {
                rotateModel.SetValue(actValue - (angle / 360), true);
            } else
            {
                rotateModel.SetValue(actValue - (angle / 360));
            }
        }
    }

    private void WriteText(float text)
    {
        if (text > 0 && text < 100)
        {
            write.text = text.ToString("0.0");
        }
    }
}
