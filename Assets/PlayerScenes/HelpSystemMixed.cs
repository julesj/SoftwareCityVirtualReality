using System.Collections; using System.Collections.Generic; using UnityEngine; using UnityEngine.SceneManagement;  public class HelpSystemMixed : MonoBehaviour {     private GameObject ui;     private GameObject scalerotate;     private GameObject move;     private GameObject choice;     private SteamVR_TrackedObject trackedObj;     private List<GameObject> trackpadIcons;     public bool[] processed;  	// Use this for initialization 	void Start () {         SceneManager.sceneLoaded += SceneManager_sceneLoaded;         trackpadIcons = new List<GameObject>();         processed = new bool[] { false, false, false, false };
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        Debug.Log(scene.name);
        if (scene.name.Equals(ClipboardBar.LoadableScenes.ScaleRotateExampleScene.ToString()))
        {
            Deactivate();
            WaitForInfoText();
        }
    }

    // Update is called once per frame
    void Update () {
        if (!ui)
        {
            ui = GameObject.Find("UI_Info");
            scalerotate = GameObject.Find("Drehen/Zoomen_Info");
            move = GameObject.Find("Minimap_Info");
            choice = GameObject.Find("Auswahl");
            
            trackpadIcons.Add(GameObject.Find("Minimap"));
            trackpadIcons.Add(GameObject.Find("ScaleRotate"));
            trackpadIcons.Add(GameObject.Find("Arrow_horizontal"));
            trackpadIcons.Add(GameObject.Find("Arrow_vertical"));

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
        Debug.Log("Wait for seconds: " + processed[0] + ", " + processed[1] + ", " + processed[2] + ", " + processed[3]);
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
            yield return new WaitForSeconds(2);
            ShowMoveInfo();
        } else if (!processed[3])
        {
            yield return new WaitForSeconds(2);
            ShowChoiceInfo();
        }
        
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
            trackpadIcons[1].SetActive(true);
            trackpadIcons[3].SetActive(true);
            scalerotate.GetComponent<PositionToHeadset>().followHeadset = false;
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(20000, Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            //Position Icons
            processed[1] = true;
        }
    }      private void ShowMoveInfo()     {//Minimap_Info         if (!processed[2])
        {
            Deactivate();
            move.SetActive(true);
            activeIcons(false);
            trackpadIcons[2].SetActive(true);
            trackpadIcons[0].SetActive(true);
            move.GetComponent<PositionToHeadset>().followHeadset = false;
            SteamVR_Controller.Input((int)trackedObj.index).TriggerHapticPulse(20000, Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            processed[2] = true;
        }
    }      private void ShowChoiceInfo()     {         if (!processed[3])
        {
            Deactivate();
            activeIcons(true);
            choice.SetActive(true);
            choice.GetComponent<PositionToHeadset>().followHeadset = false;
            processed[3] = true;
        }
    }      public void Deactivate()     {         gameObject.GetComponent<VRTK.VRTK_ControllerActions>().ToggleHighlightTouchpad(false);         GameObject help = GameObject.Find("Helptext_Mixed");         if (help)
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
        foreach (GameObject icon in trackpadIcons)
        {
            icon.SetActive(active);
        }
    } } 