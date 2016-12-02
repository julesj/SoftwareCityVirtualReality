using UnityEngine;
using System.Collections;
using VRTK;

public class CalibrationInteractionConcept : MonoBehaviour {

    private ControllerInteractionEventHandler switchCalibration;

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

            switchCalibration = new ControllerInteractionEventHandler(SwitchCalibration);
            leftController.GripPressed += switchCalibration;
            rightController.GripPressed += switchCalibration;
        }
    }

    public void OnEvent(StopInteractionConceptEvent e)
    {
        if (e.oldConcept == InteractionConcept.Calibration)
        {
            VRTK_ControllerEvents leftController = VRTK.VRTK_DeviceFinder.GetControllerLeftHand().GetComponent<VRTK_ControllerEvents>();
            VRTK_ControllerEvents rightController = VRTK.VRTK_DeviceFinder.GetControllerRightHand().GetComponent<VRTK_ControllerEvents>();

            leftController.GripPressed -= switchCalibration;
            rightController.GripPressed -= switchCalibration;
        }
    }

    public void SwitchCalibration(object sender, ControllerInteractionEventArgs e)
    {
        FindObjectOfType<LifeCycle>().GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.FinishCalibrating);
        FindObjectOfType<LifeCycle>().GetComponent<PlayingStateMachine>().PostStateEvent(StateEvent.StartCalibrating);
    }
}
