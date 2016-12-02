using UnityEngine;
using System.Collections;
using VRTK;
using System;

public class MenuController : MonoBehaviour {

	public GameObject front;
	public GameObject back;
	public GameObject left;
	public GameObject right;

	//public GameObject controller;

	//private Rigidbody rigidBody;
	private AnchorCircle anchorCircle;
    //private Transform attach;
    //private Vector2 direction = Vector2.zero;


    // Use this for initialization
    void Start () {

		//rigidBody = GetComponent<Rigidbody>();
		anchorCircle = GetComponent<AnchorCircle>();	

		anchorCircle.SetGameObjectAtAnchorpoint (0, Instantiate (front));
		anchorCircle.SetGameObjectAtAnchorpoint (1, Instantiate (right));
		anchorCircle.SetGameObjectAtAnchorpoint (2, Instantiate (back));
		anchorCircle.SetGameObjectAtAnchorpoint (3, Instantiate (left));
		//anchorCircle.RotateToAnchorPoint (2);
		//anchorCircle.RotateToAnchorPoint(0);
	}
	
    /*
	void Update () {
        if (attach == null)
        {
            attach = controller.transform.Find("Model/trackpad/attach");
            if (attach != null)
            {
                transform.position = attach.position;
                transform.rotation = attach.rotation;
                transform.parent = attach;
                
            }
        }
    }

    private void DoControllerEnabled(object sender, ControllerInteractionEventArgs e)
    {
    }

    void FixedUpdate()
    {
        if (attach != null)
        {
            float force = -direction.x * 10 * Time.deltaTime;
            //rigidBody.AddTorque(attach.transform.right * force);
        }
    }

    private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        direction = e.touchpadAxis;
    }*/
}
