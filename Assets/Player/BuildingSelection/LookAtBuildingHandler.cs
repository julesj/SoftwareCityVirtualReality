using UnityEngine;
using System.Collections;
using VRTK;

public class LookAtBuildingHandler : MonoBehaviour {

    private Ray ray;
    private float timeNextSample;
    private Building lastSelectedBuilding;
    private Vector3 lastSelectedBuildingPosition;
    private GameObject currentSelectionObject;
    
    public float samplingIntervalInSeconds = 0.25f;
    public GameObject selectionPrefab;
    public GameObject displayPrefab;

    public GameObject footstepProjectorPrefab;
    public GameObject footstepProjector;

    private Vector3 lastSelectedFloorPosition;
    private bool selectionOnFloor = false;

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
                } else if (hit.collider.transform.GetComponent<Floor>() != null)
                {
                    lastSelectedFloorPosition = hit.point;
                    selectionOnFloor = true;
                } else
                {
                    selectionOnFloor = false;
                }
            }

            /*
            if (selectionOnFloor)
            {
                if (footstepProjector == null)
                {
                    footstepProjector = (GameObject)GameObject.Instantiate(footstepProjectorPrefab);
                    footstepProjector.transform.position = new Vector3(lastSelectedFloorPosition.x, 1, lastSelectedFloorPosition.z);
                }
                else
                {
                    footstepProjector.transform.position = new Vector3(lastSelectedFloorPosition.x, 1, lastSelectedFloorPosition.z);
                }
            } else if (footstepProjector != null)
            {
                GameObject.Destroy(footstepProjector);
            }*/

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
                    currentSelectionObject.transform.localScale = bounds.size + new Vector3(0.001f, 0.001f, 0.001f);
                    //VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerActions>().ToggleHighlightTouchpad(true, new Color(0, 0, 1, 0.5f));

                    Hint.Display("BuildingSelectionConfirmHint");
                }
                else
                {
                    //VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerActions>().ToggleHighlightTouchpad(false, new Color(0, 0, 1, 0.5f));
                    transform.FindChild("TextHolder/FileNameLabel").GetComponent<TextMesh>().text = "";
                    transform.FindChild("TextHolder/PathNameLabel").GetComponent<TextMesh>().text = "";

                    if (currentSelectionObject != null)
                    {
                        Destroy(currentSelectionObject);
                        currentSelectionObject = null;
                    }

                    Hint.Hide("BuildingSelectionConfirmHint");
                }
                lastSelectedBuilding = selectedBuilding;
            }
        }
	}

    public void OnEvent(Events.ClearDisplayEvent e)
    {
        //VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerActions>().ToggleHighlightTouchpad(false, new Color(0, 0, 1, 0.5f));
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
            GameObject display = GameObject.Instantiate(displayPrefab);
            display.GetComponent<DisplayBehaviour>().SetData(lastSelectedBuilding, lastSelectedBuildingPosition, VRTK_DeviceFinder.HeadsetTransform());
            //VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerActions>().ToggleHighlightTouchpad(false, new Color(0, 0, 1, 0.5f));
            EventBus.Post(new ChangeInteractionConceptEvent(InteractionConcept.Idle));
            Hint.Confirm("BuildingSelectionConfirmHint");
            Hint.Confirm("BuildingSelectionHint");
            Hint.Confirm("BuildingSelectionTriggerHint");
        }
    }

    public void OnEvent(Events.FloorSelectionConfirmedEvent e)
    {
        /*
        if (selectionOnFloor)
        {
            VRTK_DeviceFinder.PlayAreaTransform().position = lastSelectedFloorPosition;
        }
        */
    }
}
