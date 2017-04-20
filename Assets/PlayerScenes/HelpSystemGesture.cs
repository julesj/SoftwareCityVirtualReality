using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpSystemGesture : MonoBehaviour {
    private GameObject ui;     private GameObject scalerotate;     private GameObject move;     private GameObject rotate;     private GameObject scale;     private GameObject choice;     private SteamVR_TrackedObject trackedObj;     private bool stopAll = false;     public bool[] processed = { false, false, false, false, false, false };  	// Use this for initialization 	void Start () {         SceneManager.sceneLoaded += SceneManager_sceneLoaded; 	}

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        Debug.Log(scene.name);
        if (scene.name.Equals(ClipboardBar.LoadableScenes.ScaleRotateExampleScene.ToString()))
        {
            Deactivate();
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
            rotate = GameObject.Find("Drehen_Info");
            scale = GameObject.Find("Skalieren_Info");
            choice = GameObject.Find("Auswahl");

            if (ui)
            {
                Deactivate();

                trackedObj = gameObject.GetComponent<SteamVR_TrackedObject>();

                WaitForInfoText();
            }
        }
    }      public void WaitForInfoText()
    {
        StartCoroutine(WaitForInfoText_Coroutine());
    }      private IEnumerator WaitForInfoText_Coroutine()
    {
        if (!stopAll)
        {
            if (!processed[0])
            {
                yield return new WaitForSeconds(5);
                ShowUIInfo();
            }
            else if (!processed[1])
            {
                yield return new WaitForSeconds(2);
                ShowScaleRotateInfo();
            }
            else if (!processed[2])
            {
                yield return new WaitForSeconds(5);
                ShowMoveInfo();
            }
            else if (!processed[3])
            {
                yield return new WaitForSeconds(10);
                ShowRotateInfo();
            }
            else if (!processed[4])
            {
                yield return new WaitForSeconds(10);
                ShowScaleInfo();
            }
            else if (!processed[5])
            {
                yield return new WaitForSeconds(10);
                ShowChoiceInfo();
            }
        }
    }      private IEnumerator WaitForSecond(int seconds, int processedBeTrue)
    {
        stopAll = true;
        yield return new WaitForSeconds(seconds);
        processed[processedBeTrue] = true;
        stopAll = false;
    }      private void ShowUIInfo()     {         if (!processed[0])
        {
            Deactivate();
            ui.SetActive(true);
            ui.GetComponent<PositionToHeadset>().followHeadset = false;
            processed[0] = true;
        }     }      private void ShowScaleRotateInfo()     {         if (!processed[1])
        {
            Deactivate();
            scalerotate.SetActive(true);
            scalerotate.GetComponent<PositionToHeadset>().followHeadset = false;
            StartCoroutine(WaitForSecond(5, 1));
        }
    }      private void ShowMoveInfo()     {         if (!processed[2])
        {
            Deactivate();
            move.SetActive(true);
            move.GetComponent<PositionToHeadset>().followHeadset = false;
            StartCoroutine(WaitForSecond(5, 2));
        }
    }      private void ShowRotateInfo()
    {
        if (!processed[3])
        {
            Deactivate();
            rotate.SetActive(true);
            rotate.GetComponent<PositionToHeadset>().followHeadset = false;
            StartCoroutine(WaitForSecond(5, 3));
        }
    }      private void ShowScaleInfo()
    {
        if (!processed[4])
        {
            Deactivate();
            scale.SetActive(true);
            scale.GetComponent<PositionToHeadset>().followHeadset = false;
            StartCoroutine(WaitForSecond(5, 4));
        }
    }      private void ShowChoiceInfo()     {         if (!processed[5])
        {
            Deactivate();
            choice.SetActive(true);
            choice.GetComponent<PositionToHeadset>().followHeadset = false;
            processed[5] = true;
        }
    }      public void Deactivate()     {         GameObject help = GameObject.Find("Helptext_Gesten");         if (help)
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
        }     } }
