using UnityEngine;
using System.Collections;
using System;
using VRTK;

public class CalibrationController : MonoBehaviour, InteractionConceptElement {

    public GameObject leftController;
    public GameObject rightController;

    private TableCalibrator calibrator;

    private ControllerInteractionEventHandler applicationButtonPressedHandler;

    void OnCalibrationComplete()
    {

    }

    void ApplicationMenuButtonPressed(object sender, ControllerInteractionEventArgs e)
    {
        //TODO add normalized vector to calibrator from pressed controller
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
