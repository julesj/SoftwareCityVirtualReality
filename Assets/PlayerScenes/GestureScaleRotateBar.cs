using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Leap.Unity;
using Leap;

public class GestureScaleRotateBar : MonoBehaviour {

    private bool rightHandClosed;
    private bool leftHandClosed;

    private LeapServiceProvider provider;
    private Hand hand1;
    private Hand hand2;
    private Vector3 oldPos1;
    private Vector3 oldPos2;

    private FloatModel scaleModel;
    private FloatModel rotateModel;
    private MoveSoftwareCity[] move;

    public float scaleFactor = 0.5f;

	// Use this for initialization
	void Start () {
        SceneManager.sceneLoaded += SceneLoaded;
        provider = FindObjectOfType<LeapServiceProvider>();
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Szene " + scene.name + " wurde geladen");
        if (scene.name.Equals(ClipboardBar.LoadableScenes.ScaleRotateExampleScene.ToString()))
        {
            //GetComponent<GoBack>().enabled = false;
            foreach (FloatModel model in FindObjectsOfType<FloatModel>())
            {
                if (model.name.Equals("Scale"))
                {
                    scaleModel = model;
                }
                if (model.name.Equals("Rotate"))
                {
                    rotateModel = model;
                }
            }
            move = FindObjectsOfType<MoveSoftwareCity>();
        }
    }

    // Update is called once per frame
    void Update () {
		if (rightHandClosed && leftHandClosed)
        {
            foreach (MoveSoftwareCity moveCity in move)
            {
                moveCity.SetIsGrapped(false);
            }

            Vector3 pos1 = hand1.PalmPosition.ToVector3();
            Vector3 pos2 = hand2.PalmPosition.ToVector3();
            Vector3 actual = pos1 - pos2;
            Vector3 before = oldPos1 - oldPos2;

            float angle = Mathf.Atan2(actual.z, actual.x) - Mathf.Atan2(before.z, before.x);
            float actRotateValue = rotateModel.GetValue();
            if(actRotateValue == 0.0f && angle * Mathf.Rad2Deg > 0)
            {
                rotateModel.SetValue(1.0f - Mathf.Rad2Deg * angle / 360);
            } else if (actRotateValue == 1.0f && angle * Mathf.Rad2Deg < 0)
            {
                rotateModel.SetValue(0.0f - Mathf.Rad2Deg * angle / 360);
            } else
            {
                rotateModel.SetValue(actRotateValue - Mathf.Rad2Deg * angle / 360);
            }

            float diffLength = actual.magnitude - before.magnitude;
            float actScaleValue = scaleModel.GetValue();
            scaleModel.SetValue(actScaleValue + diffLength * scaleFactor);

            oldPos1 = pos1;
            oldPos2 = pos2;
        }
	}

    public void SetRightHand(bool right)
    {
        rightHandClosed = right;
        if (rightHandClosed)
        {
            foreach (Hand hand in provider.CurrentFrame.Hands)
            {
                if (hand.IsRight)
                {
                    hand1 = hand;
                    oldPos1 = hand.PalmPosition.ToVector3();
                }
            }
        }
    }

    public void SetLeftHand(bool left)
    {
        leftHandClosed = left;
        if (leftHandClosed)
        {
            foreach (Hand hand in provider.CurrentFrame.Hands)
            {
                if (hand.IsLeft)
                {
                    hand2 = hand;
                    oldPos2 = hand.PalmPosition.ToVector3();
                }
            }
        }
    }
}
