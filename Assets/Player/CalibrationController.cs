using UnityEngine;
using System.Collections;
using System;
using VRTK;

public class CalibrationController : MonoBehaviour {

    public GameObject leftController;
    public GameObject rightController;

    public TableCalibrator calibrator;
    public AdjustableRoundTable table;

    private ControllerInteractionEventHandler applicationButtonPressedHandler;
    private ControllerInteractionEventHandler gripButtonPressedHandler;
    private ControllerInteractionEventHandler touchPadPressedHandler;

    // Life Cycle
    void Awake()
    {
        EventBus.Register(this);
    }

    void Start()
    {
        calibrator.OnCalibrationCompleteHandler += OnCalibrationComplete;
        table.gameObject.SetActive(false);
    }

    // Callbacks

    // Set calibrated values and show table
    void OnCalibrationComplete(Vector3 center, float radius)
    {
        table.height = center.y;
        table.radius = radius;

        table.gameObject.transform.position = new Vector3(center.x, 0, center.z);
        table.gameObject.SetActive(true);
    }

    // Get relevant controller component and hand it over to calibrator
    void ApplicationMenuButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        VRTK_ControllerActions controllerActions = ((VRTK_ControllerEvents)sender).gameObject.GetComponent<VRTK_ControllerActions>();
        calibrator.AddCalibrationVector(controllerActions);
    }

    // Finish Calibration Mode
    void GripButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        FindObjectOfType<LifeCycle>().GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.FinishCalibrating);
    }

    // Reset calibration and hide last calibrated table
    void TouchPadPressed(object sender, ControllerInteractionEventArgs e)
    {
        calibrator.ResetCalibration();
        table.gameObject.SetActive(false);
    }



    // Interaction Events

    public void OnEvent(StartInteractionConceptEvent c)
    {
        if (c.newConcept == InteractionConcept.Calibration)
        {
            RegisterHandler();
        }
    }

    public void OnEvent(StopInteractionConceptEvent c)
    {
        if (c.oldConcept == InteractionConcept.Calibration)
        {
            UnRegisterHandler();
        }
    }

    // Controller Event Handling Register Helper

    private void RegisterHandler()
    {
        // register ApplicationMenuButton
        if (applicationButtonPressedHandler == null)
        {
            applicationButtonPressedHandler = new ControllerInteractionEventHandler(ApplicationMenuButtonPressed);
        }
        
        //leftController.GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed += applicationButtonPressedHandler;
        //rightController.GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed += applicationButtonPressedHandler;

        // register GripButton
        if (gripButtonPressedHandler == null)
        {
            gripButtonPressedHandler = new ControllerInteractionEventHandler(GripButtonPressed);
        }

        leftController.GetComponent<VRTK_ControllerEvents>().GripPressed += gripButtonPressedHandler;
        rightController.GetComponent<VRTK_ControllerEvents>().GripPressed += gripButtonPressedHandler;

        // register TouchPad
        if (touchPadPressedHandler == null)
        {
            touchPadPressedHandler = new ControllerInteractionEventHandler(TouchPadPressed);
        }

        leftController.GetComponent<VRTK_ControllerEvents>().TouchpadPressed += touchPadPressedHandler;
        rightController.GetComponent<VRTK_ControllerEvents>().TouchpadPressed += touchPadPressedHandler;
    }

    private void UnRegisterHandler()
    {
        // unregister ApplicationMenuButton
        if (applicationButtonPressedHandler != null)
        {
            //leftController.GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed -= applicationButtonPressedHandler;
            //rightController.GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed -= applicationButtonPressedHandler;
        }

        // unregister GripButton
        if (gripButtonPressedHandler != null)
        {
            leftController.GetComponent<VRTK_ControllerEvents>().GripPressed -= gripButtonPressedHandler;
            rightController.GetComponent<VRTK_ControllerEvents>().GripPressed -= gripButtonPressedHandler;
        }

        // unregister Touchpad
        if (touchPadPressedHandler != null)
        {
            leftController.GetComponent<VRTK_ControllerEvents>().TouchpadPressed -= touchPadPressedHandler;
            rightController.GetComponent<VRTK_ControllerEvents>().TouchpadPressed -= touchPadPressedHandler;
        }
    }
}
