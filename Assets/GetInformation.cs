using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;
using Leap.Unity;
using Leap;

public class GetInformation : MonoBehaviour {
    private bool isTouched;
    private bool isHand;
    private static bool isIndexExtended;
    private Material oldMaterial;
    private GameObject currentSelectionObject;
    private GameObject canvas;
    private Transform headsetTransform;
    private LeapServiceProvider provider;
    private GameObject other;

    public GameObject selectionPrefab;

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas_Info");
	}
	
	// Update is called once per frame
	void Update () {
        if (!headsetTransform)
        {
            headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
        }
        
        if (!other)
        {
            isTouched = false;
        }

        if (isTouched && isHand)
        {
            List<Hand> hands = provider.CurrentFrame.Hands;
            if (hands.Count > 0)
            {
                Hand hand = hands[0];
                Vector3 fingerpos = GetFingersPos(hand);
                if (Mathf.Abs(gameObject.transform.InverseTransformPoint(fingerpos).z) < 1.5f)
                {
                    SetCanvas();
                    isTouched = false;
                    HelpSystemMixed helpMixed = FindObjectOfType<HelpSystemMixed>();
                    if (helpMixed.processed[3])
                    {
                        helpMixed.Deactivate();helpMixed.WaitForInfoText();
                    }

                    HelpSystemGesture helpGesture = FindObjectOfType<HelpSystemGesture>();
                    if (helpGesture && helpGesture.processed[5])
                    {
                        helpGesture.Deactivate();
                    }
                }
            }
        }

		if (isTouched && currentSelectionObject == null)
        {
            if (currentSelectionObject != null)
            {
                Destroy(currentSelectionObject);
            }
            currentSelectionObject = (GameObject)GameObject.Instantiate(selectionPrefab);
            Bounds bounds = gameObject.GetComponent<Renderer>().bounds;
            currentSelectionObject.transform.position = bounds.center;
            currentSelectionObject.transform.localScale = transform.parent.lossyScale + new Vector3(0.001f, 0.001f, 0.001f);
            currentSelectionObject.transform.rotation = GameObject.Find("SoftwareCity").transform.rotation;
        } else if (!isTouched && currentSelectionObject != null)
        {
            Destroy(currentSelectionObject);
            currentSelectionObject = null;
        }
	}

    private void SetCanvas()
    {
        Debug.Log("Set Canvas");
        canvas.SetActive(true);
        Dictionary<string, string> attributes = gameObject.GetComponentInParent<Building>().node.allAttributes;
        List<string> keys = new List<string>();
        keys.AddRange(attributes.Keys);
        keys.Sort();
        string list = "";
        foreach (string key in keys)
        {
            list += key + ": " + attributes[key] + "\n";
        }

        GameObject.Find("PackageLabel").GetComponent<Text>().text = gameObject.GetComponentInParent<Building>().node.pathName;
        GameObject.Find("NameLabel").GetComponent<Text>().text = gameObject.GetComponentInParent<Building>().node.name;
        GameObject.Find("Infotext").GetComponent<Text>().text = list;

        canvas.transform.position = headsetTransform.position + headsetTransform.forward * 0.5f;
        canvas.transform.LookAt(headsetTransform);
    }

    private Vector3 GetFingersPos(Hand hand)
    {
        List<Finger> fingerList = hand.Fingers;
        foreach (Finger finger in fingerList)
        {
            if (finger.Type == Finger.FingerType.TYPE_INDEX)
            {
                return finger.TipPosition.ToVector3();
            }
        }
        return Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("[Controller (right)]StraightPointerRenderer_Cursor") || other.name.Equals("[Controller (left)]StraightPointerRenderer_Cursor"))
        {
            isTouched = true;
            VRTK_ControllerEvents events = other.gameObject.GetComponentInParent<VRTK_ControllerEvents>();
            events.TriggerClicked -= Events_TriggerClicked;
            events.TriggerClicked += Events_TriggerClicked;
            this.other = other.gameObject;
        }
        if (other.name.Equals("bone1") || other.name.Equals("bone2") || other.name.Equals("bone3"))
        {
            if (isIndexExtended)
            {
                isTouched = true;
                isHand = true;
                provider = FindObjectOfType<LeapServiceProvider>();
                this.other = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("[Controller (right)]StraightPointerRenderer_Cursor") || other.name.Equals("[Controller (left)]StraightPointerRenderer_Cursor"))
        {
            isTouched = false;
            VRTK_ControllerEvents events = other.gameObject.GetComponentInParent<VRTK_ControllerEvents>();
            events.TriggerClicked -= Events_TriggerClicked;
        }
        if (other.name.Equals("bone1") || other.name.Equals("bone2") || other.name.Equals("bone3"))
        {
            isTouched = false;
            isHand = false;
        }
    }

    private void Events_TriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        if (isTouched)
        {
            SetCanvas();
            isTouched = false;
            if (FindObjectOfType<HelpSystemController>().processed[3])
            {
                FindObjectOfType<HelpSystemController>().Deactivate();
            }
        }
    }

    public void SetIsIndexExtended(bool extended)
    {
        isIndexExtended = extended;
    }
}
