using UnityEngine;
using System.Collections;
using System;
using VRTK;

public class CalibrationController : MonoBehaviour, InteractionConceptElement {

    public GameObject leftController;
    public GameObject rightController;

    private TableCalibrator calibrator;
    private AdjustableRoundTable table;

    private ControllerInteractionEventHandler applicationButtonPressedHandler;

    void OnCalibrationComplete(Vector3 center, float radius)
    {
        table.height = center.y;
        table.radius = radius;
        gameObject.transform.position = new Vector3(center.x, 0, center.z);
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
        calibrator = gameObject.GetComponent<TableCalibrator>();
        calibrator.OnCalibrationCompleteHandler += OnCalibrationComplete;

        table = gameObject.GetComponent<AdjustableRoundTable>();
    }

    // Interaction Concept Element Impl

    public void ActivateElement()
    {
        RegisterHandler();
    }

    public void DeactivateElement()
    {
        UnRegisterHandler();
    }

    public InteractionConcept GetConcept()
    {
        return InteractionConcept.Calibration;
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
