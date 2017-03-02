using UnityEngine;
using System.Collections;
using VRTK;

public class CalibrationInteractionConcept : MonoBehaviour {

    private ControllerInteractionEventHandler startCalibration;

    void Awake()
    {
        EventBus.Register(this);
    }

    public void OnEvent(StartInteractionConceptEvent e)
    {
        if (e.newConcept == InteractionConcept.ReadyForCalibration)
        {
            VRTK_ControllerEvents leftController = VRTK.VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_ControllerEvents>();
            VRTK_ControllerEvents rightController = VRTK.VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>();

            startCalibration = new ControllerInteractionEventHandler(StartCalibration);
            leftController.GripPressed += startCalibration;
            rightController.GripPressed += startCalibration;
        }
    }

    public void OnEvent(StopInteractionConceptEvent e)
    {
        if (e.oldConcept == InteractionConcept.ReadyForCalibration)
        {
            VRTK_ControllerEvents leftController = VRTK.VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_ControllerEvents>();
            VRTK_ControllerEvents rightController = VRTK.VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>();

            leftController.GripPressed -= startCalibration;
            rightController.GripPressed -= startCalibration;
        }
    }

    public void StartCalibration(object sender, ControllerInteractionEventArgs e)
    {
        FindObjectOfType<LifeCycle>().GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.StartCalibrating);
    }
}
