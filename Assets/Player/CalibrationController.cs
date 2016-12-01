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

    void Awake()
    {
        EventBus.Register(this);
    }

    void OnCalibrationComplete(Vector3 center, float radius)
    {
        table.height = center.y;
        table.radius = radius;

        table.gameObject.transform.position = new Vector3(center.x, 0, center.z);
        table.gameObject.SetActive(true);
    }

    void ApplicationMenuButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        //TODO add normalized vector to calibrator from pressed controller
        VRTK_ControllerActions controllerActions = ((VRTK_ControllerEvents)sender).gameObject.GetComponent<VRTK_ControllerActions>();
        calibrator.AddCalibrationVector(controllerActions);
    }

    // Life Cycle

    void Start()
    {
        calibrator.OnCalibrationCompleteHandler += OnCalibrationComplete;
    }

    // Interaction Concept Element Impl

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

    // Controller Event Handling Helper

    private void RegisterHandler()
    {
        if (applicationButtonPressedHandler == null)
        {
            applicationButtonPressedHandler = new ControllerInteractionEventHandler(ApplicationMenuButtonPressed);
        }
        
        leftController.GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed += applicationButtonPressedHandler;
        rightController.GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed += applicationButtonPressedHandler;
    }

    private void UnRegisterHandler()
    {
        if (applicationButtonPressedHandler != null)
        {
            leftController.GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed -= applicationButtonPressedHandler;
            rightController.GetComponent<VRTK_ControllerEvents>().ApplicationMenuPressed -= applicationButtonPressedHandler;
        }
    }
}
