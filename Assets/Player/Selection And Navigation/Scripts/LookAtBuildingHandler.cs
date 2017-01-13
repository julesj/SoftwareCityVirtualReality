using UnityEngine;
using System.Collections;
using VRTK;
using UnityStandardAssets.ImageEffects;

public class LookAtBuildingHandler : MonoBehaviour {

    private Ray ray;
    private float timeNextSample;
    private Building lastSelectedBuilding;
    private Vector3 lastSelectedBuildingPosition;
    private GameObject currentSelectionObject;

    private Vector3 lastSelectedFloorPosition;
    private bool selectionOnFloor = false;
    private GameObject navigationHint;

    public float samplingIntervalInSeconds = 0.25f;
    public GameObject selectionPrefab;
    public GameObject displayPrefab;
    public GameObject navigationHintPrefab;


    void Start () {
        EventBus.Register(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > timeNextSample)
        {
            ray.origin = transform.position;
            ray.direction = transform.forward;
            timeNextSample = Time.time + samplingIntervalInSeconds;

            float nearestDist = float.MaxValue;
            Building selectedBuilding = null;

            Decoration cube = null;

            foreach (RaycastHit hit in Physics.RaycastAll(ray, 100))
            {
                if (hit.distance > nearestDist)
                {
                    continue;
                }
                nearestDist = hit.distance;
                if (hit.collider.transform.GetComponent<Building>() != null)
                {
                    selectedBuilding = hit.collider.transform.GetComponent<Building>();
                    lastSelectedBuildingPosition = hit.point;
                }
                if (hit.collider.transform.GetComponent<Floor>() != null)
                {
                    lastSelectedFloorPosition = hit.point;
                    selectionOnFloor = true;
                } else
                {
                    selectionOnFloor = false;
                }
                if (hit.collider.transform.GetComponent<Decoration>() != null)
                {
                    cube = hit.collider.transform.GetComponent<Decoration>();
                }
                else
                {
                    cube = null;
                }
            }

            if (selectionOnFloor)
            {
                if (navigationHint == null)
                {
                    navigationHint = (GameObject)GameObject.Instantiate(navigationHintPrefab);
                }
                navigationHint.transform.position = new Vector3(lastSelectedFloorPosition.x, 0, lastSelectedFloorPosition.z);
                navigationHint.transform.rotation = Quaternion.LookRotation(new Vector3(ray.direction.x, 0, ray.direction.z).normalized, Vector3.up);
            } else if (navigationHint != null)
            {
                GameObject.Destroy(navigationHint);
            }

            if (cube != null)
            {
                cube.TractorBeamToPosition(ray.origin);
            }

            if (selectedBuilding != lastSelectedBuilding)
            {
                if (selectedBuilding != null)
                {
                    transform.FindChild("TextHolder/FileNameLabel").GetComponent<TextMesh>().text = selectedBuilding.node.name;
                    transform.FindChild("TextHolder/PathNameLabel").GetComponent<TextMesh>().text = selectedBuilding.node.pathName;
                    AnimateThis anim = transform.FindChild("TextHolder").GetComponent<AnimateThis>();
                    anim.CancelAll();
                    anim.Transformate().Duration(1).FromScale(new Vector3(1, 0, 1)).ToScale(Vector3.one).Ease(AnimateThis.EaseOutElastic).Start();

                    if (currentSelectionObject != null)
                    {
                        Destroy(currentSelectionObject);
                    }
                    currentSelectionObject = (GameObject) GameObject.Instantiate(selectionPrefab);
                    Bounds bounds = selectedBuilding.gameObject.GetComponent<Renderer>().bounds;
                    currentSelectionObject.transform.position = bounds.center;
                    currentSelectionObject.transform.localScale = selectedBuilding.transform.parent.lossyScale + new Vector3(0.001f, 0.001f, 0.001f);
                    currentSelectionObject.transform.rotation = GameObject.Find("SoftwareCity").transform.rotation;

                    VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerActions>().TriggerHapticPulse((ushort) (0.1f * 3999));

                    Hint.Display("BuildingSelectionConfirmHint");
                    //VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerActions>().ToggleHighlightTouchpad(true, new Color(0, 0, 1, 0.5f));
                }
                else
                {
                    transform.FindChild("TextHolder/FileNameLabel").GetComponent<TextMesh>().text = "";
                    transform.FindChild("TextHolder/PathNameLabel").GetComponent<TextMesh>().text = "";

                    if (currentSelectionObject != null)
                    {
                        Destroy(currentSelectionObject);
                        currentSelectionObject = null;
                    }

                    Hint.Hide("BuildingSelectionConfirmHint");
                    //VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerActions>().ToggleHighlightTouchpad(false);
                }
                lastSelectedBuilding = selectedBuilding;
            }
        }
	}

    public void OnEvent(StartInteractionConceptEvent e)
    {
        if (e.newConcept == InteractionConcept.Selection)
        {
            Hint.Display("HowToNavigateHint");
        }
    }

    

    public void OnEvent(StopInteractionConceptEvent e)
    {
        if (e.oldConcept == InteractionConcept.Selection)
        {
            Hint.Hide("HowToNavigateHint");
            if (navigationHint != null)
            {
                Destroy(navigationHint);
            }
            if (currentSelectionObject != null)
            {
                Destroy(currentSelectionObject);
            }
        }
    }

    public void OnEvent(Events.ClearDisplayEvent e)
    {
        //VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerActions>().ToggleHighlightTouchpad(false);
        if (currentSelectionObject != null)
        {
            Destroy(currentSelectionObject);
            currentSelectionObject = null;
        }
    }

    public void OnEvent(Events.BuildingSelectionConfirmedEvent e)
    {
        if (lastSelectedBuilding != null)
        {
            // TODO: Instantiate without position leads to flickering around zero origin when display shows up
            GameObject display = GameObject.Instantiate(displayPrefab);
            display.GetComponent<DisplayBehaviour>().SetData(lastSelectedBuilding, lastSelectedBuildingPosition, VRTK_DeviceFinder.HeadsetTransform());
            //VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerActions>().ToggleHighlightTouchpad(false);
            EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Idle));
            Hint.Confirm("BuildingSelectionConfirmHint");
            Hint.Confirm("BuildingSelectionHint");
            Hint.Confirm("BuildingSelectionTriggerHint");
        }
        if (selectionOnFloor)
        {
            BlurOptimized blur = VRTK_DeviceFinder.HeadsetCamera().GetComponent<BlurOptimized>();
            blur.enabled = true;
            
            Transform playArea = VRTK_DeviceFinder.PlayAreaTransform();
            Hint.Confirm("HowToNavigateHint");

            AnimateThis playAreaAnimation = playArea.GetComponent<AnimateThis>();
            playAreaAnimation.CancelAll();
            playAreaAnimation.Transformate()
                .ToPosition(lastSelectedFloorPosition)
                .Duration(0.2f)
                .Ease(AnimateThis.EaseInQuintic)
                .OnEnd(OnNavigationAnimationComplete)
                .Start();
        }
    }

    public void OnNavigationAnimationComplete()
    {
        BlurOptimized blur = VRTK_DeviceFinder.HeadsetCamera().GetComponent<BlurOptimized>();
        blur.enabled = false;
    }
}
