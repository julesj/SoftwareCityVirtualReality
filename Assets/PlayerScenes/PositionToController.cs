using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionToController : MonoBehaviour {
    private Transform trackpad;
    private GameObject controller;
    private bool positioned = false;
    public bool positionToRightController;
    public bool positionToLeftController;
    public enum positionToController
    {
        centerTrackpad,
        upTrackpad,
        downTrackpad,
        rightTrackpad,
        leftTrackpad
    };

    public positionToController posToController;

	// Update is called once per frame
	void Update () {
        if (!controller)
        {
            if (positionToRightController)
            {
                controller = GameObject.Find("Controller (right)");
            }
            else if (positionToLeftController)
            {
                controller = GameObject.Find("Controller (left)");
            }
        } else
        {
            if (!trackpad)
            {
                trackpad = controller.transform.GetChild(0).FindChild("trackpad");
            }
        }

        if (trackpad && !positioned)
        {
            PositionToTrackpad();
        }
	}

    private void PositionToTrackpad()
    {
        transform.parent = trackpad;
        if (posToController == positionToController.centerTrackpad)
        {
            transform.localPosition = new Vector3(0, 0.001f, -0.045f);
            transform.localEulerAngles = new Vector3(90, 180, 180);
        }
        if (posToController == positionToController.upTrackpad)
        {
            transform.localPosition = new Vector3(0, 0.01f, -0.02f);
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        if (posToController == positionToController.downTrackpad)
        {
            transform.localPosition = new Vector3(0, 0.01f, -0.075f);
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        if (posToController == positionToController.rightTrackpad)
        {
            transform.localPosition = new Vector3(0.03f, 0.01f, -0.05f);
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        if (posToController == positionToController.leftTrackpad)
        {
            transform.localPosition = new Vector3(-0.03f, 0.01f, -0.05f);
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }

        positioned = true;
    }
}
