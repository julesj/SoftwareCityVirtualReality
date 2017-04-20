﻿using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.SceneManagement; using VRTK;  public class HelpSystemController : MonoBehaviour {     private GameObject ui;     private GameObject scalerotate;     private GameObject move;     private GameObject choice;     private SteamVR_TrackedObject trackedObj;     private VRTK_ControllerActions actions;     private List<GameObject> leftIcons;     private List<GameObject> rightIcons;     public bool[] processed;  	// Use this for initialization 	void Start () {         SceneManager.sceneLoaded += SceneManager_sceneLoaded;         leftIcons = new List<GameObject>();         rightIcons = new List<GameObject>();         actions = gameObject.GetComponent<VRTK_ControllerActions>();
        processed = new bool[] { false, false, false, false };
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        Debug.Log(scene.name);
        if (scene.name.Equals(ClipboardBar.LoadableScenes.ScaleRotateExampleScene.ToString()))
        {
            if (!processed[0])
            {
                processed[0] = true;
            }
            WaitForInfoText();
        }
    }

    // Update is called once per frame
    void Update () {
        if (!ui)
        {
            ui = GameObject.Find("UI_Info");
            scalerotate = GameObject.Find("Drehen/Zoomen_Info");
            move = GameObject.Find("Bewegen");
            choice = GameObject.Find("Auswahl");

            leftIcons.Add(GameObject.Find("Zoom_Out_Left"));
            leftIcons.Add(GameObject.Find("Zoom_In_Left"));
            leftIcons.Add(GameObject.Find("Rotate_Left_Left"));
            leftIcons.Add(GameObject.Find("Rotate_Right_Left"));

            rightIcons.Add(GameObject.Find("Zoom_In_Right"));
            rightIcons.Add(GameObject.Find("Zoom_Out_Right"));
            rightIcons.Add(GameObject.Find("Rotate_Left_Right"));
            rightIcons.Add(GameObject.Find("Rotate_Right_Right"));

            if (ui)
            {
                Deactivate();
                activeIcons(false);

                trackedObj = gameObject.GetComponent<SteamVR_TrackedObject>();

                WaitForInfoText();
            }
        }
    }      public void WaitForInfoText()
    {
        StartCoroutine(WaitForInfoText_Coroutine());
    }      private IEnumerator WaitForInfoText_Coroutine()
    {
        if (!processed[0])
        {
            yield return new WaitForSeconds(5);
            ShowUIInfo();
        } else if (!processed[1])
        {
            yield return new WaitForSeconds(5);
            ShowScaleRotateInfo();
        } else if (!processed[2])
        {
            yield return new WaitForSeconds(5);
            ShowMoveInfo();
        } else if (!processed[3])
        {
            yield return new WaitForSeconds(5);
            ShowChoiceInfo();
        }
        
    }      private void ShowUIInfo()     {         if (!processed[0])
        {
            Deactivate();
            ui.SetActive(true);
            ui.GetComponent<PositionToHeadset>().followHeadset = false;
            actions.ToggleHighlightTrigger(true, Color.cyan, 0.5f);
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(2000, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
            processed[0] = true;
        }     }      private void ShowScaleRotateInfo()     {         if (!processed[1])
        {
            Deactivate();
            scalerotate.SetActive(true);
            activeIcons(true);
            scalerotate.GetComponent<PositionToHeadset>().followHeadset = false;
            actions.ToggleHighlightTouchpad(true, Color.cyan, 0.5f);
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(2000, Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            processed[1] = true;
        }
    }      private void ShowMoveInfo()     {         if (!processed[2])
        {
            Deactivate();
            move.SetActive(true);
            move.GetComponent<PositionToHeadset>().followHeadset = false;
            actions.ToggleHighlightGrip(true, Color.cyan, 0.5f);
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(2000, Valve.VR.EVRButtonId.k_EButton_Grip);
            processed[2] = true;
        }
    }      private void ShowChoiceInfo()     {         if (!processed[3])
        {

            Deactivate();
            choice.SetActive(true);
            choice.GetComponent<PositionToHeadset>().followHeadset = false;
            actions.ToggleHighlightTrigger(true, Color.cyan, 0.5f);
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(2000, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
            processed[3] = true;
        }
    }      public void Deactivate()     {         actions.ToggleHighlightTrigger(false);         actions.ToggleHighlightGrip(false);         actions.ToggleHighlightTouchpad(false);         GameObject help = GameObject.Find("Helptext_Controller");         if (help)
        {
            Transform[] children = help.GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                if (child.gameObject.GetComponent<PositionToHeadset>())
                {
                    child.gameObject.GetComponent<PositionToHeadset>().followHeadset = true;
                    child.gameObject.SetActive(false);
                }
            }
        }     }      private void activeIcons(bool active)
    {
        foreach (GameObject icon in leftIcons)
        {
            icon.SetActive(active);
        }
        foreach (GameObject icon in rightIcons)
        {
            icon.SetActive(active);
        }
    } } 