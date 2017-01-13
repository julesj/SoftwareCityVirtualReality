using UnityEngine;
using System.Collections;
using VRTK;

public class TableFactory : MonoBehaviour {

    public GameObject leftController;
    public GameObject rightController;

    private bool leftControllerClicked;
    private bool rightControllerClicked;

    private Transform tableTop;
    private Transform tableMiddle;
    private Transform tableBottom;

    void Start () {

        if (leftController == null || rightController == null)
        {
            Debug.LogError("no controller connected");
            return;
        }
        
        leftController.GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(LeftControllerDoTriggerClicked);
        leftController.GetComponent<VRTK_ControllerEvents>().TriggerUnclicked += new ControllerInteractionEventHandler(LeftControllerDoTriggerUnclicked);

        rightController.GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(RightControllerDoTriggerClicked);
        rightController.GetComponent<VRTK_ControllerEvents>().TriggerUnclicked += new ControllerInteractionEventHandler(RightControllerDoTriggerUnclicked);

        tableTop = transform.Find("TableTop");
        tableMiddle = transform.Find("TableMiddle");
        tableBottom = transform.Find("TableBottom");
    }
	
	void Update () {
	    if (leftControllerClicked && rightControllerClicked)
        {
            Debug.Log("Resize Table in progress");
            Transform left = leftController.transform.Find("Model/base/attach");
            Transform right = rightController.transform.Find("Model/base/attach");

            float distance = Vector3.Distance(left.position, right.position);

            Vector3 center = Vector3.Lerp(left.position, right.position, 0.5f);

            Debug.Log("center: " + center.ToString() + " distance: " + distance);
            Debug.Log("left: " + left.ToString() + " right: " + right.ToString());

            AdjustableTableMesh topMesh = tableTop.GetComponent<AdjustableTableMesh>();
            topMesh.bottomRadius = distance / 2;
            topMesh.topRadius = distance / 2;
            topMesh.UpdateMesh();
            tableTop.transform.position = center;

            AdjustableTableMesh middleMesh = tableMiddle.GetComponent<AdjustableTableMesh>();
            middleMesh.height = center.y;
            middleMesh.UpdateMesh();

            AdjustableTableMesh bottomMesh = tableBottom.GetComponent<AdjustableTableMesh>();
            bottomMesh.UpdateMesh();

            transform.position = new Vector3(center.x,0,center.z);
        }
	}

    private void LeftControllerDoTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        leftControllerClicked = true;
    }

    private void LeftControllerDoTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        leftControllerClicked = false;
    }

    private void RightControllerDoTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        rightControllerClicked = true;
    }

    private void RightControllerDoTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        rightControllerClicked = false;
    }
}
