using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using Leap.Unity;
using Leap;

public class GetInformation : MonoBehaviour {
    public bool isTouched;
    private bool isHand;
    private bool isClicked;
    private Material oldMaterial;
    private GameObject currentSelectionObject;
    private GameObject canvas;
    private Transform headsetTransform;
    private LeapServiceProvider provider;

    public GameObject selectionPrefab;

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas_Info");
        //canvas.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (!headsetTransform)
        {
            headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
        }

        if (isTouched && isHand)
        {
            List<Hand> hands = provider.CurrentFrame.Hands;
            if (hands.Count > 0)
            {
                Hand hand = hands[0];
                Vector3 fingerpos = GetFingersPos(hand);
                if (Mathf.Abs(gameObject.transform.InverseTransformPoint(fingerpos).z) < 5)
                {
                    isClicked = true;
                } else
                {
                    isClicked = false;
                }
            }
        }

		if (isTouched && currentSelectionObject == null)
        {
            Debug.Log("IsTouched");
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

        if (isTouched && isClicked)
        {
            canvas.SetActive(true);
            //canvas.gameObject.GetComponentInChildren<Text>() = gameObject.GetComponentInParent<Building>().node.allAttributes;
            canvas.transform.position = headsetTransform.position + headsetTransform.forward * 0.5f;
            canvas.transform.LookAt(headsetTransform);
        }
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
            events.TriggerClicked += Events_TriggerClicked;
            events.TriggerReleased += Events_TriggerReleased;
        }
        if (other.name.Equals("bone1") || other.name.Equals("bone2") || other.name.Equals("bone3"))
        {
            isTouched = true;
            isHand = true;
            provider = FindObjectOfType<LeapServiceProvider>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("[Controller (right)]StraightPointerRenderer_Cursor") || other.name.Equals("[Controller (left)]StraightPointerRenderer_Cursor"))
        {
            isTouched = false;
            VRTK_ControllerEvents events = other.gameObject.GetComponentInParent<VRTK_ControllerEvents>();
            events.TriggerClicked -= Events_TriggerClicked;
            events.TriggerReleased -= Events_TriggerReleased;
        }
        if (other.name.Equals("bone1") || other.name.Equals("bone2") || other.name.Equals("bone3"))
        {
            isTouched = false;
            isHand = false;
        }
    }

    private void Events_TriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        isClicked = false;
    }

    private void Events_TriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        isClicked = true;
    }

    public void SetIsTouched(bool touched)
    {
        Debug.Log("SetIsTouched " + touched);
        isTouched = touched;
    }
}
