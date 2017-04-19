using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpSystemController : MonoBehaviour {
    private GameObject ui;
    private GameObject scalerotate;
    private GameObject move;
    private GameObject choice;

	// Use this for initialization
	void Start () {
        ui = GameObject.Find("UI_Info");
        scalerotate = GameObject.Find("Drehen/Zoomen_Info");
        move = GameObject.Find("Bewegen");
        choice = GameObject.Find("Auswahl");
        Deactivate();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowUIInfo()
    {
        Deactivate();
        ui.SetActive(true);
        ui.GetComponent<PositionToHeadset>().followHeadset = false;
        //Haptisches Feedback Trigger
    }

    public void ShowScaleRotateInfo()
    {
        Deactivate();
        scalerotate.SetActive(true);
        scalerotate.GetComponent<PositionToHeadset>().followHeadset = false;
        //Haptisches Feedback Trackpad
        //Position Icons
    }

    public void ShowMoveInfo()
    {
        Deactivate();
        move.SetActive(true);
        move.GetComponent<PositionToHeadset>().followHeadset = false;
        //Haptisches Feedback Grips
    }

    public void ShowChoiceInfo()
    {
        Deactivate();
        choice.SetActive(true);
        choice.GetComponent<PositionToHeadset>().followHeadset = false;
        //Haptisches Feedback Trigger
    }

    private void Deactivate()
    {
        GameObject help = GameObject.Find("Helptext");
        Transform[] children = help.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            child.gameObject.GetComponent<PositionToHeadset>().followHeadset = true;
            child.gameObject.SetActive(false);
        }
    }
}
